import PropTypes from 'prop-types';
import { Line } from 'react-chartjs-2';
import 'chart.js/auto';
import { Paper, Box, Alert } from '@mui/material';
import 'chartjs-adapter-date-fns';
import dayjs from 'dayjs';

const Chart = ({ data, selectedOption, isAggregated, aggregationType, error, granularity }) => {
  const labels = {
    temperature: 'Temperature (°C)',
    pressure: 'Pressure (hPa)',
    humidity: 'Humidity (%)',
  };

  const colors = {
    temperature: 'rgb(255, 99, 132)',
    pressure: 'rgb(54, 162, 235)',
    humidity: 'rgb(75, 192, 192)',
  };

  if (error) {
    return (
      <Alert severity="error">{error}</Alert>
    );
  }

  if (selectedOption === '') {
    return (
      <Alert severity="info">Please select a Data Type.</Alert>
    );
  }

  if (data.length === 0) {
    return (
      <Alert severity="info">No data available for the selected time span.</Alert>
    );
  }

  const displayFormats = {
    minute: 'HH:mm',
    hour: 'MMM dd HH:00',
    day: 'MMM dd',
  };

  const unit = {
    minute: 'minute',
    hour: 'hour',
    day: 'day',
  };

  const chartData = {
    labels: data.map(entry => dayjs(entry.timestamp).format()),
    datasets: [
      {
        label: labels[selectedOption],
        data: data.map(entry => entry[selectedOption]),
        fill: false,
        backgroundColor: colors[selectedOption],
        borderColor: colors[selectedOption],
        borderWidth: 1,
        tension: 0,
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        type: 'time',
        time: {
          unit: unit[granularity],
          tooltipFormat: 'Pp',
          displayFormats: {
            [unit[granularity]]: displayFormats[granularity],
          },
        },
        title: {
          display: true,
          text: 'Time',
        },
      },
      y: {
        title: {
          display: true,
          text: labels[selectedOption],
        },
      },
    },
    plugins: {
      legend: {
        display: true,
        labels: {
          font: {
            size: 14,
          },
        },
      },
      title: {
        display: true,
        text: `Sensor Data (${isAggregated ? `${aggregationType} average` : 'Raw'})`,
        font: {
          size: 18,
        },
      },
    },
    elements: {
      line: {
        borderWidth: 2,
      },
      point: {
        radius: 3,
      },
    },
  };

  return (
    <Paper elevation={3} sx={{ height: '100%', width: '100%' }}>
      <Box p={2} sx={{ height: '100%', width: '100%' }}>
        <Box sx={{ height: { xs: '300px', sm: '400px', md: '500px', lg: '600px' } }}>
          <Line data={chartData} options={chartOptions} />
        </Box>
      </Box>
    </Paper>
  );
};

Chart.propTypes = {
  data: PropTypes.arrayOf(PropTypes.object).isRequired,
  selectedOption: PropTypes.string.isRequired,
  isAggregated: PropTypes.bool.isRequired,
  aggregationType: PropTypes.string,
  error: PropTypes.string,
  granularity: PropTypes.string.isRequired,
  timeSpan: PropTypes.string.isRequired,
};

export default Chart;

import React from 'react';
import { Line } from 'react-chartjs-2';
import 'chart.js/auto';
import { Paper, Box, Typography } from '@mui/material';
import 'chartjs-adapter-date-fns';

const Chart = ({ data, selectedOption, isAggregated, error, granularity }) => {
  const labels = {
    temperature: 'Temperature (°C)',
    pressure: 'Pressure (hPa)',
    humidity: 'Humidity (%RH)',
  };

  const colors = {
    temperature: 'rgb(255, 99, 132)',
    pressure: 'rgb(54, 162, 235)',
    humidity: 'rgb(75, 192, 192)',
  };

  if (error) {
    return (
      <Paper elevation={3}>
        <Box p={2} height="400px" display="flex" justifyContent="center" alignItems="center">
          <Typography variant="h6" color="error">
            {error}
          </Typography>
        </Box>
      </Paper>
    );
  }

  if (!selectedOption || data.length === 0) {
    return (
      <Paper elevation={3}>
        <Box p={2} height="400px" display="flex" justifyContent="center" alignItems="center">
          <Typography variant="h6">
            No data fetched.
          </Typography>
        </Box>
      </Paper>
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
    labels: data.map(entry => entry.timestamp), // Use raw timestamps for Chart.js to format
    datasets: [
      {
        label: labels[selectedOption],
        data: data.map((entry) => entry[selectedOption]),
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
    scales: {
      x: {
        type: 'time',
        time: {
          unit: unit[granularity], // Dynamic unit based on granularity prop
          tooltipFormat: 'Pp',
          displayFormats: {
            [unit[granularity]]: displayFormats[granularity], // Dynamic display format
          },
        },
        title: {
          display: true,
          text: 'Timestamp',
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
        text: `Sensor Data (${isAggregated ? 'Aggregated' : 'Raw'})`,
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
    <Paper elevation={3}>
      <Box p={2}>
        <Line data={chartData} options={chartOptions} />
      </Box>
    </Paper>
  );
};

export default Chart;

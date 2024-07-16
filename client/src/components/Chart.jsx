import React from 'react';
import { Line } from 'react-chartjs-2';
import 'chart.js/auto';
import { Paper, Box } from '@mui/material';

const Chart = ({ data, selectedOption, error }) => {
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
          {error}
        </Box>
      </Paper>
    );
  }

  if (!selectedOption || data.length === 0) {
    return (
      <Paper elevation={3}>
        <Box p={2} height="400px" display="flex" justifyContent="center" alignItems="center">
          No data available. Please select a data type.
        </Box>
      </Paper>
    );
  }

  const chartLabels = data.map((entry) =>
    new Date(entry.timestamp).toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false,
    })
  );

  const chartData = {
    labels: chartLabels,
    datasets: [
      {
        label: labels[selectedOption],
        data: data.map((entry) => entry[selectedOption]),
        fill: false,
        backgroundColor: colors[selectedOption],
        borderColor: colors[selectedOption],
        borderWidth: 2,
        tension: 0,
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    scales: {
      x: {
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

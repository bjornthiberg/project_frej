import React from 'react';
import { Line } from 'react-chartjs-2';
import Chart from 'chart.js/auto';

const LineChart = ({ data, selectedOption }) => {
  if (!selectedOption) return null;

  const chartLabels = data.map((entry) =>
    new Date(entry.timestamp).toLocaleString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false, // Use 24-hour format
    })
  );

  const chartData = {
    labels: chartLabels,
    datasets: [
      {
        label: {
          temperature: 'Temperature (°C)',
          pressure: 'Pressure (hPa)',
          humidity: 'Humidity (%RG)',
          lux: 'Lux',
        }[selectedOption],
        data: data.map((entry) => entry[selectedOption]),
        fill: false,
        backgroundColor: {
          temperature: 'rgb(255, 99, 132)',
          pressure: 'rgb(54, 162, 235)',
          humidity: 'rgb(75, 192, 192)',
          lux: 'rgb(153, 102, 255)',
        }[selectedOption],
        borderColor: {
          temperature: 'rgba(255, 99, 132, 0.2)',
          pressure: 'rgba(54, 162, 235, 0.2)',
          humidity: 'rgba(75, 192, 192, 0.2)',
          lux: 'rgba(153, 102, 255, 0.2)',
        }[selectedOption],
      },
    ],
  };

  const options = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        ticks: {
          maxTicksLimit: 10, // Show fewer labels
        },
      },
      y: {
        beginAtZero: false,
      },
    },
  };

  return (
    <div style={{ height: '100%', width: '100%' }}>
      <Line data={chartData} options={options} />
    </div>
  );
};

export default LineChart;

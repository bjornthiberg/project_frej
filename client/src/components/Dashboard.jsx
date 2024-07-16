import React, { useState, useEffect } from 'react';
import { Container, Grid } from '@mui/material';
import ChartFilterBar from './ChartFilterBar';
import Chart from './Chart';
import FetchSensorData from '../services/FetchSensorData';
import dayjs from 'dayjs';

const Dashboard = () => {
  const [chartConfig, setChartConfig] = useState({
    option: '',
    timeRange: 'hour',
  });

  const [customDates, setCustomDates] = useState({
    start: dayjs().subtract(1, 'day'),
    end: dayjs(),
  });

  const [data, setData] = useState([]);
  const [error, setError] = useState(null);

  const options = [
    { label: 'Temperature (°C)', value: 'temperature' },
    { label: 'Humidity (%RG)', value: 'humidity' },
    { label: 'Pressure (hPa)', value: 'pressure' },
  ];

  useEffect(() => {
    if (chartConfig.option) {
      FetchSensorData(chartConfig.option, chartConfig.timeRange, customDates.start, customDates.end)
        .then(({ data, error }) => {
          setData(data);
          setError(error);
        });
    }
  }, [chartConfig, customDates]);

  const handleChartConfigChange = (key, value) => {
    setChartConfig((prevConfig) => ({
      ...prevConfig,
      [key]: value,
    }));
  };

  const handleCustomDateChange = (key, value) => {
    setCustomDates((prevDates) => ({
      ...prevDates,
      [key]: value,
    }));

    if (chartConfig.timeRange === 'custom' && chartConfig.option) {
      FetchSensorData(chartConfig.option, 'custom', customDates.start, customDates.end)
        .then(({ data, error }) => {
          setData(data);
          setError(error);
        });
    }
  };

  return (
    <Container>
      <Grid container spacing={3} style={{ marginTop: 20 }}>
        <Grid item xs={12}>
          <ChartFilterBar
            selectedOption={chartConfig.option}
            handleOptionChange={(value) => handleChartConfigChange('option', value)}
            options={options}
            selectedTimeRange={chartConfig.timeRange}
            handleTimeRangeChange={(value) => handleChartConfigChange('timeRange', value)}
            customStartDate={customDates.start}
            customEndDate={customDates.end}
            handleCustomDateChange={handleCustomDateChange}
          />
        </Grid>
        <Grid item xs={12}>
          <Chart data={data} selectedOption={chartConfig.option} error={error} />
        </Grid>
      </Grid>
    </Container>
  );
};

export default Dashboard;

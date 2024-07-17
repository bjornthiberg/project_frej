import React, { useState, useEffect } from 'react';
import { Container, Grid, Paper, Alert, Box } from '@mui/material';
import ChartFilterBar from './ChartFilterBar';
import Chart from './Chart';
import CustomDatePicker from './CustomDatePicker';
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
  const [isAggregated, setIsAggregated] = useState(false);
  const [granularity, setGranularity] = useState('minute');

  const options = [
    { label: 'Temperature (°C)', value: 'temperature' },
    { label: 'Humidity (%RG)', value: 'humidity' },
    { label: 'Pressure (hPa)', value: 'pressure' },
  ];

  const determineGranularity = (timeRange, start, end) => {
    if (timeRange === 'hour') return 'minute';
    if (timeRange === 'day') return 'hour';
    if (timeRange === 'week') return 'day';
    if (timeRange === 'custom') {
      const diffInHours = dayjs(end).diff(dayjs(start), 'hour');
      if (diffInHours <= 2) return 'minute';
      if (diffInHours <= 24 * 7) return 'hour';
      return 'day';
    }
    return 'minute';
  };

  useEffect(() => {
    if (chartConfig.option) {
      const { timeRange, option } = chartConfig;
      const { start, end } = customDates;

      const newGranularity = determineGranularity(timeRange, start, end);
      setGranularity(newGranularity);

      FetchSensorData(option, timeRange, start, end).then(({ data, error }) => {
        setData(data);
        setError(error);

        if (timeRange === 'custom') {
          const diffInHours = dayjs(end).diff(dayjs(start), 'hour');
          setIsAggregated(diffInHours >= 2);
        } else {
          setIsAggregated(timeRange !== 'hour');
        }
      });
    }
  }, [chartConfig, customDates]);

  const handleChartConfigChange = (key, value) => {
    setChartConfig((prevConfig) => ({
      ...prevConfig,
      [key]: value,
    }));

    if (key === 'timeRange' && value === 'custom' && chartConfig.option) {
      const { start, end } = customDates;
      const newGranularity = determineGranularity('custom', start, end);
      setGranularity(newGranularity);

      FetchSensorData(chartConfig.option, 'custom', start, end).then(({ data, error }) => {
        setData(data);
        setError(error);
        const diffInHours = dayjs(end).diff(dayjs(start), 'hour');
        setIsAggregated(diffInHours >= 2);
      });
    }
  };

  const handleCustomDateChange = (key, value) => {
    setCustomDates((prevDates) => ({
      ...prevDates,
      [key]: value,
    }));
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
          {error ? (
            <Alert severity="error">{error}</Alert>
          ) : !data.length ? (
            <Alert severity="info">No data fetched.</Alert>
          ) : (
            <Chart
              data={data}
              selectedOption={chartConfig.option}
              isAggregated={isAggregated}
              error={error}
              granularity={granularity}
            />
          )}
        </Grid>
        <Grid item xs={12}>
          {chartConfig.timeRange === 'custom' && (
            <Box mt={2} display="flex" justifyContent="center">
              <CustomDatePicker
                customStartDate={customDates.start}
                customEndDate={customDates.end}
                handleCustomDateChange={handleCustomDateChange}
              />
            </Box>
          )}
        </Grid>
      </Grid>
    </Container>
  );
};

export default Dashboard;

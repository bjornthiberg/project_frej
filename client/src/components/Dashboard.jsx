import { useState, useEffect } from 'react';
import { Container, Grid, Box, Paper } from '@mui/material';
import LiveDataBar from './LiveDataBar';
import ChartFilterBar from './ChartFilterBar';
import Chart from './Chart';
import CustomDatePicker from './CustomDatePicker';
import FetchSensorData from '../services/FetchSensorData';
import dayjs from 'dayjs';
import { fetchDailyAverage, fetchLatestReading } from '../services/FetchLiveData';

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
  const [timeSpan, setTimeSpan] = useState('');
  const [aggregationType, setAggregationType] = useState("");
  const [liveData, setLiveData] = useState({
    temperature: 0,
    humidity: 0,
    pressure: 0,
    temperatureAvg: 1,
    humidityAvg: 1,
    pressureAvg: 1,
  });

  const options = [
    { label: 'Temperature (°C)', value: 'temperature' },
    { label: 'Humidity (%)', value: 'humidity' },
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
    const fetchData = async () => {
      const yesterday = dayjs().subtract(1, 'day').format('YYYY-MM-DD');
      const averageData = await fetchDailyAverage(yesterday);
      const latestReading = await fetchLatestReading();

      setLiveData({
        temperature: latestReading.temperature,
        humidity: latestReading.humidity,
        pressure: latestReading.pressure,
        temperatureAvg: averageData.avgTemperature,
        humidityAvg: averageData.avgHumidity,
        pressureAvg: averageData.avgPressure,
      });
    };

    fetchData();

    const interval = setInterval(() => {
      fetchData();
    }, 10000);

    return () => clearInterval(interval);
  }, []);


  useEffect(() => {
    if (chartConfig.option) {
      const { timeRange, option } = chartConfig;
      const { start, end } = customDates;

      const newGranularity = determineGranularity(timeRange, start, end);
      setGranularity(newGranularity);

      FetchSensorData(option, timeRange, start, end).then(({ data, error, isAggregated, aggregationType, timeSpan }) => {
        setData(data);
        setError(error);
        setIsAggregated(isAggregated);
        setAggregationType(aggregationType);
        setTimeSpan(timeSpan);
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

      FetchSensorData(chartConfig.option, 'custom', start, end).then(({ data, error, isAggregated, aggregationType, timeSpan }) => {
        setData(data);
        setError(error);
        setIsAggregated(isAggregated);
        setAggregationType(aggregationType);
        setTimeSpan(timeSpan);
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
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <LiveDataBar data={liveData} />
        </Grid>
        <Grid item xs={12}>
          <Paper elevation={3} sx={{ padding: '16px', backgroundColor: 'transparent' }}>
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
            <Chart
              data={data}
              selectedOption={chartConfig.option}
              isAggregated={isAggregated}
              aggregationType={aggregationType}
              error={error}
              granularity={granularity}
              timeSpan={timeSpan}
            />
            {chartConfig.timeRange === 'custom' && (
              <Box mt={2} display="flex" justifyContent="center">
                <CustomDatePicker
                  customStartDate={customDates.start}
                  customEndDate={customDates.end}
                  handleCustomDateChange={handleCustomDateChange}
                />
              </Box>
            )}
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default Dashboard;

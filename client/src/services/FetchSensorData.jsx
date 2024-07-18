import axios from 'axios';
import dayjs from 'dayjs';

const baseUrl = "https://frejapi.thiberg.dev/api";

const mapAggregateData = (data, option) => {
    const avgKey = `avg${option.charAt(0).toUpperCase() + option.slice(1)}`;
    return data
        .filter(entry => entry.hour || entry.date) // Filter out entries without valid timestamps
        .map((entry) => ({
            timestamp: entry.hour || entry.date,
            [option]: entry[avgKey],
        }))
        .filter(entry => dayjs(entry.timestamp).isValid()); // Filter out entries with invalid dates
};

const fetchHourlyAggregatesLast24Hours = async (date, option) => {
    const hourlyData = [];
    let currentDate = dayjs(date).subtract(24, 'hours');

    for (let hour = 0; hour < 24; hour++) {
        const url = `${baseUrl}/sensorData/aggregate/hourly/${currentDate.format('YYYY-MM-DD')}/${currentDate.hour()}`;
        try {
            const response = await axios.get(url);
            if (response.data) {
                hourlyData.push(response.data);
            }
        } catch (error) {
            console.error('Error fetching hourly aggregate:', error);
            return { data: [], error: 'Error fetching hourly aggregate data', isAggregated: true, aggregationType: 'hourly', timeSpan: 'last 24 hours' };
        }
        currentDate = currentDate.add(1, 'hour');
    }
    return { data: mapAggregateData(hourlyData, option), error: null, isAggregated: true, aggregationType: 'hourly', timeSpan: 'last 24 hours' };
};

const fetchHourlyAggregates = async (startDate, endDate, option) => {
    const hourlyData = [];
    let currentDate = dayjs(startDate).startOf('hour');

    while (currentDate.isBefore(endDate) || currentDate.isSame(endDate)) {
        const dateFormatted = currentDate.format('YYYY-MM-DD');
        const hourFormatted = currentDate.hour();

        const url = `${baseUrl}/sensorData/aggregate/hourly/${dateFormatted}/${hourFormatted}`;
        try {
            const response = await axios.get(url);
            if (response.data) {
                hourlyData.push(response.data);
            }
        } catch (error) {
            console.error('Error fetching hourly aggregate:', error);
            const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD HH:mm');
            const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD HH:mm');
            return { data: [], error: 'Error fetching hourly aggregate data', isAggregated: true, aggregationType: 'hourly', timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
        }
        currentDate = currentDate.add(1, 'hour');
    }
    const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD HH:mm');
    const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD HH:mm');
    return { data: mapAggregateData(hourlyData, option), error: null, isAggregated: true, aggregationType: 'hourly', timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
};

const fetchDailyAggregates = async (startDate, nDays, option) => {
    const dailyData = [];
    const endDate = dayjs(startDate).add(nDays, 'day').format('YYYY-MM-DD'); // Define endDate here
    for (let day = 0; day < nDays; day++) {
        const date = dayjs(startDate).add(day, 'day').format('YYYY-MM-DD');
        const url = `${baseUrl}/sensorData/aggregate/daily/${date}`;
        try {
            const response = await axios.get(url);
            if (response.data) {
                dailyData.push(response.data);
            }
        } catch (error) {
            console.error('Error fetching daily aggregate:', error);
            const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD');
            const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD');
            return { data: [], error: 'Error fetching daily aggregate data', isAggregated: true, aggregationType: 'daily', timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
        }
    }
    const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD');
    const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD');
    return { data: mapAggregateData(dailyData, option), error: null, isAggregated: true, aggregationType: 'daily', timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
};

const FetchSensorData = async (option, timeRange, customStartDate, customEndDate) => {
    let startDate;
    let endDate;
    let url;

    try {
        if (timeRange === 'hour') {
            startDate = dayjs().subtract(1, 'hour').format('YYYY-MM-DDTHH:mm:ss');
            endDate = dayjs().format('YYYY-MM-DDTHH:mm:ss');
            url = `${baseUrl}/sensorData/date-range/${startDate}/${endDate}`;
        } else if (timeRange === 'day') {
            const date = dayjs().format('YYYY-MM-DDTHH:00');
            return await fetchHourlyAggregatesLast24Hours(date, option);
        } else if (timeRange === 'week') {
            startDate = dayjs().subtract(1, 'week').format('YYYY-MM-DD');
            endDate = dayjs().format('YYYY-MM-DD');
            return await fetchDailyAggregates(startDate, 7, option);
        } else if (timeRange === 'custom' && customStartDate && customEndDate) {
            startDate = dayjs(customStartDate).format('YYYY-MM-DDTHH:mm:ss');
            endDate = dayjs(customEndDate).format('YYYY-MM-DDTHH:mm:ss');
            const timeRangeSize = dayjs(endDate).diff(dayjs(startDate), 'hours');

            if (timeRangeSize <= 2) {
                url = `${baseUrl}/sensorData/date-range/${startDate}/${endDate}`;
            } else if (timeRangeSize <= 24 * 7) {
                return await fetchHourlyAggregates(startDate, endDate, option);
            } else {
                const start = dayjs(customStartDate).format('YYYY-MM-DD');
                const days = dayjs(endDate).diff(dayjs(startDate), 'day');
                return await fetchDailyAggregates(start, days, option);
            }
        }

        const response = await axios.get(url, { params: { type: option } });
        const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD HH:mm:ss');
        const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD HH:mm:ss');
        return { data: response.data, error: null, isAggregated: false, aggregationType: null, timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
    } catch (error) {
        console.error('Error fetching sensor data:', error);
        const formattedStartDate = dayjs(startDate).format('YYYY-MM-DD HH:mm:ss');
        const formattedEndDate = dayjs(endDate).format('YYYY-MM-DD HH:mm:ss');
        return { data: [], error: 'Error fetching sensor data', isAggregated: false, aggregationType: null, timeSpan: `${formattedStartDate} to ${formattedEndDate}` };
    }
};

export default FetchSensorData;

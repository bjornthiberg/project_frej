import axios from 'axios';
import dayjs from 'dayjs';

const baseUrl = 'https://frejapi.thiberg.dev/api';

const getStartDate = (timeRange) => {
    const now = dayjs();
    switch (timeRange) {
        case 'hour':
            return now.subtract(1, 'hour').format('YYYY-MM-DDTHH:mm');
        case 'day':
            return now.subtract(1, 'day').format('YYYY-MM-DDTHH:mm');
        case 'week':
            return now.subtract(1, 'week').format('YYYY-MM-DDTHH:mm');
        default:
            throw new Error('Invalid time range');
    }
};

const FetchSensorData = async (option, timeRange, customStartDate, customEndDate) => {
    let startDate;
    let endDate;

    if (timeRange === 'custom' && customStartDate && customEndDate) {
        startDate = dayjs(customStartDate).format('YYYY-MM-DDTHH:mm');
        endDate = dayjs(customEndDate).format('YYYY-MM-DDTHH:mm');
    } else {
        startDate = getStartDate(timeRange);
        endDate = dayjs().format('YYYY-MM-DDTHH:mm');
    }

    const url = `${baseUrl}/sensorData/date-range/${startDate}/${endDate}`;

    try {
        const response = await axios.get(url, { params: { type: option } });
        return { data: response.data, error: null };
    } catch (error) {
        console.error('Error fetching sensor data:', error);
        return { data: [], error: 'Error fetching sensor data' };
    }
};

export default FetchSensorData;

import axios from 'axios';
import dayjs from 'dayjs';

const baseUrl = "https://homefrejapi.thiberg.dev/api";

const fetchDailyAverage = async (date) => {
    try {
        const formattedDate = dayjs(date).format('YYYY-MM-DD');
        const url = `${baseUrl}/sensordata/aggregate/daily/${formattedDate}`;
        const response = await axios.get(url);
        const data = response.data;

        return {
            avgTemperature: data.avgTemperature ?? null,
            avgHumidity: data.avgHumidity ?? null,
            avgPressure: data.avgPressure ?? null,
        };
    } catch (error) {
        console.error('Error fetching daily average data:', error);
        return {
            avgTemperature: null,
            avgHumidity: null,
            avgPressure: null,
        };
    }
};

const fetchLatestReading = async () => {
    try {
        const url = `${baseUrl}/sensordata?pageSize=1`;
        const response = await axios.get(url);
        const latestData = response.data.data[0];

        return {
            temperature: latestData.temperature ?? null,
            humidity: latestData.humidity ?? null,
            pressure: latestData.pressure ?? null,
        };
    } catch (error) {
        console.error('Error fetching latest reading:', error);
        return {
            temperature: null,
            humidity: null,
            pressure: null,
        };
    }
};

export { fetchDailyAverage, fetchLatestReading };

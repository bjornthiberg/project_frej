import axios from 'axios';
import dayjs from 'dayjs';

const baseUrl = "https://homefrejapi.thiberg.dev/api";

const fetchDailyAverage = async (date) => {
    const formattedDate = dayjs(date).format('YYYY-MM-DD');
    const url = `${baseUrl}/sensordata/aggregate/daily/${formattedDate}`;
    const response = await axios.get(url);
    const data = response.data;

    return {
        avgTemperature: data.avgTemperature,
        avgHumidity: data.avgHumidity,
        avgPressure: data.avgPressure,
    };
};

const fetchLatestReading = async () => {
    const url = `${baseUrl}/sensordata?pageSize=1`;
    const response = await axios.get(url);
    return response.data.data[0];
};

export { fetchDailyAverage, fetchLatestReading };

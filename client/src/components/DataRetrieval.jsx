import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CircularProgress from '@mui/material/CircularProgress';
import LineChart from './LineChart';

const DataRetrieval = ({ endpoint, selectedOption, pageSize }) => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await axios.get(endpoint);
                const sensorData = response.data.data.slice(-pageSize);
                setData(sensorData);
            } catch (error) {
                console.error('Error fetching data:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [endpoint, selectedOption, pageSize]);

    return loading ? <CircularProgress /> : <LineChart data={data} selectedOption={selectedOption} />;
};

export default DataRetrieval;

import React, { useEffect, useState } from 'react';
import axios from 'axios';
import CircularProgress from '@mui/material/CircularProgress';
import LineChart from './LineChart';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

const DataRetrieval = ({ endpoint, selectedOption, pageSize }) => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                setError(null);

                const response = await axios.get(`${endpoint}/latest?pageSize=${pageSize}`);
                const sensorData = response.data.data;

                setData(sensorData);
            } catch (error) {
                console.error('Error fetching data:', error);
                setError('Failed to fetch data. Please try again later.');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [endpoint, selectedOption, pageSize]);

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="100%">
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="100%">
                <Typography color="error" variant="h6">
                    {error}
                </Typography>
            </Box>
        );
    }

    return <LineChart data={data} selectedOption={selectedOption} />;
};

export default DataRetrieval;

import PropTypes from 'prop-types';
import { useEffect, useState } from 'react';
import { Box, Typography, Grid, Paper, Avatar, Tooltip } from '@mui/material';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import TrendingFlatIcon from '@mui/icons-material/TrendingFlat';
import TrendingDownIcon from '@mui/icons-material/TrendingDown';
import ThermostatIcon from '@mui/icons-material/Thermostat';
import OpacityIcon from '@mui/icons-material/Opacity';
import SpeedIcon from '@mui/icons-material/Speed';
import { keyframes } from '@emotion/react';
import { css } from '@emotion/css';

const expandContractAnimation = keyframes`
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
`;

const animationClass = css`
  animation: ${expandContractAnimation} 0.5s ease-in-out;
`;

const LiveDataBar = ({ data }) => {
    const { temperature, humidity, pressure, temperatureAvg, humidityAvg, pressureAvg } = data;

    const [animateClass, setAnimateClass] = useState('');

    useEffect(() => {
        setAnimateClass(animationClass);
        const timer = setTimeout(() => {
            setAnimateClass('');
        }, 500);

        return () => clearTimeout(timer);
    }, [temperature, humidity, pressure]);

    const getTrendIcon = (current, avg, color) => {
        if (current > avg) {
            return <TrendingUpIcon style={{ color, fontSize: '16px', verticalAlign: 'middle' }} />;
        } else if (current < avg) {
            return <TrendingDownIcon style={{ color, fontSize: '16px', verticalAlign: 'middle' }} />;
        } else {
            return <TrendingFlatIcon style={{ color, fontSize: '16px', verticalAlign: 'middle' }} />;
        }
    };

    const renderDataBox = (label, icon, current, avg, unit, color, bgColor) => (
        <Grid item xs={12} sm={6} md={4}>
            <Paper elevation={3} sx={{ padding: '8px 12px', backgroundColor: bgColor }}>
                <Box display="flex" alignItems="center">
                    <Avatar sx={{ bgcolor: color, marginRight: '8px', width: 32, height: 32 }}>
                        {icon}
                    </Avatar>
                    <Box textAlign="center" flex={1}>
                        <Typography variant="subtitle1" style={{ color, fontSize: '14px' }}>{label}</Typography>
                        <Typography variant="h6" className={animateClass} style={{ color, fontSize: '16px' }}>
                            {current.toFixed(2)}{unit} ({getTrendIcon(current, avg, color)}
                            <Tooltip title={`Compared to average yesterday: ${avg.toFixed(2)}${unit}`}>
                                <Typography component="span" variant="subtitle2" style={{ marginLeft: '4px', color, fontSize: '12px', lineHeight: '16px' }}>
                                    {((current - avg) / avg * 100).toFixed(2)}%
                                </Typography>)
                            </Tooltip>
                        </Typography>
                    </Box>
                </Box>
            </Paper>
        </Grid>
    );

    return (
        <Grid container spacing={2}>
            {renderDataBox('Temperature', <ThermostatIcon style={{ fontSize: '20px' }} />, temperature, temperatureAvg, '°C', 'rgb(255, 99, 132)', 'rgba(255, 99, 132, 0.2)')}
            {renderDataBox('Humidity', <OpacityIcon style={{ fontSize: '20px' }} />, humidity, humidityAvg, '%', 'rgb(75, 192, 192)', 'rgba(75, 192, 192, 0.2)')}
            {renderDataBox('Air Pressure', <SpeedIcon style={{ fontSize: '20px' }} />, pressure, pressureAvg, ' hPa', 'rgb(54, 162, 235)', 'rgba(54, 162, 235, 0.2)')}
        </Grid>
    );
};

export default LiveDataBar;

LiveDataBar.propTypes = {
    data: PropTypes.shape({
        temperature: PropTypes.number.isRequired,
        humidity: PropTypes.number.isRequired,
        pressure: PropTypes.number.isRequired,
        temperatureAvg: PropTypes.number.isRequired,
        humidityAvg: PropTypes.number.isRequired,
        pressureAvg: PropTypes.number.isRequired,
    }).isRequired,
};

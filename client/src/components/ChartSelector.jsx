import React from 'react';
import { Paper, Typography } from '@mui/material';

const ChartSelector = ({ title }) => {
  return (
    <Paper style={{ padding: 20, height: 400 }}>
      <Typography variant="h6">{title}</Typography>
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80%' }}>
        Placeholder for chart
      </div>
    </Paper>
  );
};

export default ChartSelector;

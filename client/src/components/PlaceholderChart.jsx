import React from 'react';
import { Paper, Typography} from '@mui/material';

const PlaceholderChart = ({ title }) => (
  <Paper style={{ padding: 20, height: 400 }}>
    <Typography variant="h6">{title}</Typography>
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
      <p>:(</p>
    </div>
  </Paper>
);

export default PlaceholderChart;

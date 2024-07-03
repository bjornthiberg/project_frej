import React from 'react';
import { Container, Grid } from '@mui/material';
import ChartSelector from '../components/ChartSelector';

const DashboardLayout = () => {
  return (
    <Container>
      <Grid container spacing={3} style={{ marginTop: 20 }}>
        <Grid item xs={12} md={6}>
          <ChartSelector title="Chart 1" />
        </Grid>
        <Grid item xs={12} md={6}>
          <ChartSelector title="Chart 2" />
        </Grid>
        <Grid item xs={12} md={6}>
          <ChartSelector title="Chart 3" />
        </Grid>
        <Grid item xs={12} md={6}>
          <ChartSelector title="Chart 4" />
        </Grid>
      </Grid>
    </Container>
  );
};

export default DashboardLayout;

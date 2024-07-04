import React, { useState } from 'react';
import { Container, Grid } from '@mui/material';
import ChartSelector from '../components/ChartSelector';

const DashboardLayout = () => {
  const initialOptionsState = ['', '', '', ''];
  const initialPageSizeState = [25, 25, 25, 25];
  const [selectedOptions, setSelectedOptions] = useState(initialOptionsState);
  const [pageSizes, setPageSizes] = useState(initialPageSizeState);

  const handleChange = (index) => (event) => {
    const newSelectedOptions = [...selectedOptions];
    newSelectedOptions[index] = event.target.value;
    setSelectedOptions(newSelectedOptions);
  };

  const handlePageSizeChange = (index) => (event) => {
    const newPageSizes = [...pageSizes];
    newPageSizes[index] = event.target.value;
    setPageSizes(newPageSizes);
  };

  const options = [
    { label: 'Temperature (°C)', value: 'temperature' },
    { label: 'Humidity (%RG)', value: 'humidity' },
    { label: 'Pressure (hPa)', value: 'pressure' },
    { label: 'Lux', value: 'lux' },
  ];

  return (
    <Container>
      <Grid container spacing={3} style={{ marginTop: 20 }}>
        {selectedOptions.map((selectedOption, index) => (
          <Grid item xs={12} md={6} key={index}>
            <ChartSelector
              options={options}
              selectedOption={selectedOption}
              handleChange={handleChange(index)}
              pageSize={pageSizes[index]}
              handlePageSizeChange={handlePageSizeChange(index)}
            />
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default DashboardLayout;

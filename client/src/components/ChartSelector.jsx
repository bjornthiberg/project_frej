import React from 'react';
import { Paper, Typography, Select, MenuItem, FormControl, InputLabel } from '@mui/material';
import DataRetrieval from './DataRetrieval';
import PageSizeSelector from './PageSizeSelector';

const ChartSelector = ({ options, selectedOption, handleChange, pageSize, handlePageSizeChange }) => {
  const baseUrl = import.meta.env.VITE_API_BASE_URL;

  return (
    <div>
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '8px' }}>
        <div style={{ display: 'flex', alignItems: 'center' }}>
          <FormControl variant="outlined" size="small" style={{ minWidth: 120, marginRight: '20px' }}>
            <InputLabel id="chart-select-label">Select</InputLabel>
            <Select
              labelId="chart-select-label"
              value={selectedOption}
              onChange={handleChange}
              label="Select"
            >
              {options.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <PageSizeSelector pageSize={pageSize} onPageSizeChange={handlePageSizeChange} />
        </div>
      </div>
      <Paper style={{ padding: 20, height: 400 }}>
        <div style={{ height: '100%', width: '100%' }}>
          {selectedOption && (
            <DataRetrieval endpoint={`${baseUrl}/sensorData`} selectedOption={selectedOption} pageSize={pageSize} />
          )}
        </div>
      </Paper>
    </div>
  );
};

export default ChartSelector;

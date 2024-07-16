import React from 'react';
import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';

const DataTypePicker = ({ selectedOption, handleOptionChange, options }) => {
    return (
        <FormControl variant="outlined" size="small" style={{ minWidth: 120, marginRight: '20px' }}>
            <InputLabel id="chart-select-label">Data Type</InputLabel>
            <Select
                labelId="chart-select-label"
                value={selectedOption}
                onChange={(e) => handleOptionChange(e.target.value)}
                label="Data Type"
            >
                {options.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                        {option.label}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
};

export default DataTypePicker;

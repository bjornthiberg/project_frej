import React from 'react';
import { FormControl, InputLabel, MenuItem, Select } from '@mui/material';

const PageSizeSelector = ({ pageSize, onPageSizeChange }) => {
    return (
        <FormControl variant="outlined" size="small" style={{ minWidth: 120, marginRight: '20px' }}>
            <InputLabel id="page-size-select-label">Number of Items</InputLabel>
            <Select
                labelId="page-size-select-label"
                value={pageSize}
                onChange={onPageSizeChange}
                label="Number of Items"
            >
                <MenuItem value={10}>10</MenuItem>
                <MenuItem value={25}>25</MenuItem>
                <MenuItem value={50}>50</MenuItem>
                <MenuItem value={100}>100</MenuItem>
            </Select>
        </FormControl>
    );
};

export default PageSizeSelector;

import React from 'react';
import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';

const PageSizeSelector = ({ pageSize, onPageSizeChange }) => {
    return (
        <FormControl variant="outlined" size="small" style={{ minWidth: 120, marginRight: '20px' }}>
            <InputLabel id="page-size-select-label">Page Size</InputLabel>
            <Select
                labelId="page-size-select-label"
                value={pageSize}
                onChange={onPageSizeChange}
                label="Page Size"
            >
                {[10, 25, 50, 100, 250, 500].map(size => (
                    <MenuItem key={size} value={size}>{size}</MenuItem>
                ))}
            </Select>
        </FormControl>
    );
};

export default PageSizeSelector;

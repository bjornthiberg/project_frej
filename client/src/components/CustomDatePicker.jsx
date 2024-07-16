import React from 'react';
import { Box } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';

const CustomDatePicker = ({ customStartDate, customEndDate, handleCustomDateChange }) => {
    return (
        <Box display="flex" alignItems="center" gap={2}>
            <DateTimePicker
                label="Start Date"
                value={customStartDate}
                onChange={(newValue) => handleCustomDateChange('start', newValue)}
                ampm={false}
                slotProps={{ textField: { variant: 'outlined', size: 'small', sx: { minWidth: 120 } } }}
            />
            <DateTimePicker
                label="End Date"
                value={customEndDate}
                onChange={(newValue) => handleCustomDateChange('end', newValue)}
                ampm={false}
                slotProps={{ textField: { variant: 'outlined', size: 'small', sx: { minWidth: 120 } } }}
            />
        </Box>
    );
};

export default CustomDatePicker;

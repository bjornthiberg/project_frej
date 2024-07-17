import React from 'react';
import { Box } from '@mui/material';
import TimeToggleButtons from './TimeToggleButtons';
import DataTypePicker from './DataTypePicker';

const ChartFilterBar = ({
    selectedOption,
    handleOptionChange,
    options,
    selectedTimeRange,
    handleTimeRangeChange,
}) => {
    return (
        <Box display="flex" alignItems="center" justifyContent="space-between" marginBottom="8px">
            <DataTypePicker
                selectedOption={selectedOption}
                handleOptionChange={handleOptionChange}
                options={options}
            />
            <TimeToggleButtons
                selectedTimeRange={selectedTimeRange}
                onTimeRangeChange={handleTimeRangeChange}
            />
        </Box>
    );
};

export default ChartFilterBar;

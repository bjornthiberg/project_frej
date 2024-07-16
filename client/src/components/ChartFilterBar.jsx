import React, { useState } from 'react';
import { Box } from '@mui/material';
import TimeToggleButtons from './TimeToggleButtons';
import CustomDatePicker from './CustomDatePicker';
import DataTypePicker from './DataTypePicker';

const ChartFilterBar = ({
    selectedOption,
    handleOptionChange,
    options,
    selectedTimeRange,
    handleTimeRangeChange,
    customStartDate,
    customEndDate,
    handleCustomDateChange,
}) => {
    const [showDatePicker, setShowDatePicker] = useState(false);

    const handleRangeChange = (range) => {
        setShowDatePicker(range === 'custom');
        handleTimeRangeChange(range);
    };

    return (
        <Box display="flex" alignItems="center" justifyContent="space-between" marginBottom="8px">
            <DataTypePicker
                selectedOption={selectedOption}
                handleOptionChange={handleOptionChange}
                options={options}
            />
            {showDatePicker && (
                <CustomDatePicker
                    customStartDate={customStartDate}
                    customEndDate={customEndDate}
                    handleCustomDateChange={handleCustomDateChange}
                />
            )}
            <TimeToggleButtons
                selectedTimeRange={selectedTimeRange}
                onTimeRangeChange={handleRangeChange}
            />
        </Box>
    );
};

export default ChartFilterBar;

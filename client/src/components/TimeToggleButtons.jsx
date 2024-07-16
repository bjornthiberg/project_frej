import React from "react";
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';

export default function TimeToggleButtons({ selectedTimeRange, onTimeRangeChange }) {
    const handleRangeChange = (event, newRange) => {
        if (newRange !== null) {
            onTimeRangeChange(newRange);
        }
    };

    return (
        <ToggleButtonGroup
            value={selectedTimeRange}
            exclusive
            onChange={handleRangeChange}
            aria-label="time range"
        >
            <ToggleButton value="custom" aria-label="custom" variant="outlined">
                Custom
            </ToggleButton>
            <ToggleButton value="hour" aria-label="hour" variant="outlined">
                Hour
            </ToggleButton>
            <ToggleButton value="day" aria-label="day" variant="outlined">
                Day
            </ToggleButton>
            <ToggleButton value="week" aria-label="week" variant="outlined">
                Week
            </ToggleButton>
        </ToggleButtonGroup>
    );
}

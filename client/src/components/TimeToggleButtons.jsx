import PropTypes from 'prop-types';
import { ToggleButton, ToggleButtonGroup, Tooltip } from '@mui/material';

function TimeToggleButtons({ selectedTimeRange, onTimeRangeChange }) {
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
            <Tooltip title="Select a custom date range">
                <ToggleButton value="custom" aria-label="custom" variant="outlined">
                    Custom
                </ToggleButton>
            </Tooltip>
            <Tooltip title="View data from the last hour">
                <ToggleButton value="hour" aria-label="hour" variant="outlined">
                    Hour
                </ToggleButton>
            </Tooltip>
            <Tooltip title="View data from the last 24 hours">
                <ToggleButton value="day" aria-label="day" variant="outlined">
                    Day
                </ToggleButton>
            </Tooltip>
            <Tooltip title="View data from the last 7 days">
                <ToggleButton value="week" aria-label="week" variant="outlined">
                    Week
                </ToggleButton>
            </Tooltip>
        </ToggleButtonGroup>
    );
}

TimeToggleButtons.propTypes = {
    selectedTimeRange: PropTypes.string.isRequired,
    onTimeRangeChange: PropTypes.func.isRequired,
};

export default TimeToggleButtons;

import PropTypes from 'prop-types';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';

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

TimeToggleButtons.propTypes = {
    selectedTimeRange: PropTypes.string.isRequired,
    onTimeRangeChange: PropTypes.func.isRequired,
};

export default TimeToggleButtons;

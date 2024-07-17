import PropTypes from 'prop-types';
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

ChartFilterBar.propTypes = {
    selectedOption: PropTypes.string.isRequired,
    handleOptionChange: PropTypes.func.isRequired,
    options: PropTypes.arrayOf(PropTypes.shape({
        value: PropTypes.string.isRequired,
        label: PropTypes.string.isRequired,
    })).isRequired,
    selectedTimeRange: PropTypes.string.isRequired,
    handleTimeRangeChange: PropTypes.func.isRequired,
};

export default ChartFilterBar;

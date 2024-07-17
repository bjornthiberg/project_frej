import PropTypes from 'prop-types';
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

DataTypePicker.propTypes = {
    selectedOption: PropTypes.string.isRequired,
    handleOptionChange: PropTypes.func.isRequired,
    options: PropTypes.arrayOf(PropTypes.shape({
        value: PropTypes.string.isRequired,
        label: PropTypes.string.isRequired,
    })).isRequired,
}

export default DataTypePicker;

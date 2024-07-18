import PropTypes from 'prop-types';
import { FormControl, InputLabel, Select, MenuItem, Tooltip } from '@mui/material';

const DataTypePicker = ({ selectedOption, handleOptionChange, options }) => {
    return (
        <Tooltip title="Select a data type to view" placement="top">
            <FormControl variant="outlined" size="medium" style={{ minWidth: 120 }}>
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
        </Tooltip>
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

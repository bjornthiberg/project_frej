import React from 'react';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';

const Footer = () => {
    return (
        <Box
            component="footer"
            sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                padding: '16px',
                marginTop: 'auto',
                borderRadius: '0px'
            }}
        >
            <Typography variant="body2">
                &copy; {new Date().getFullYear()} Björn Thiberg
            </Typography>
        </Box>
    );
};

export default Footer;

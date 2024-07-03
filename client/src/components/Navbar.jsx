// src/components/Navbar.jsx
import React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';

function Navbar() {
  return (
    <AppBar position="static" sx={ { borderRadius: '0px' } }>
      <Toolbar>
        <Typography variant="h6">
          Project Frej - Sensor Data
        </Typography>
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;

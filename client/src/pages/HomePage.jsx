import React from 'react';
import Navbar from '../components/Navbar';
import Dashboard from '../components/Dashboard';
import Footer from '../components/Footer';
import Box from '@mui/material/Box';

const HomePage = () => {
  return (
    <Box
      display="flex"
      flexDirection="column"
      minHeight="100vh"
    >
      <Navbar />
      <Box
        component="main"
        flexGrow={1}
        padding={3}
      >
        <Dashboard />
      </Box>
      <Footer />
    </Box>
  );
};

export default HomePage;

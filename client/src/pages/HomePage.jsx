import React from 'react';
import Navbar from '../components/Navbar';
import DashboardLayout from '../components/DashboardLayout';
import Footer from '../components/Footer';

const HomePage = () => {
  return (
    <div>
      <Navbar />
      <DashboardLayout />
      <Footer />
    </div>
  );
};

export default HomePage;

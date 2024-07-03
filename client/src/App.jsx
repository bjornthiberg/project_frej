import React from 'react';
import { CssBaseline } from '@mui/material';

import HomePage from './pages/HomePage';
import './App.css';

export default function App() {
  return (
    <React.Fragment>
      <CssBaseline />
      <div className="App">
        <HomePage />
      </div>
    </React.Fragment>
  );
}

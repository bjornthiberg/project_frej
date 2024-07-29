import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  typography: {
    fontFamily: "'Public Sans', 'Helvetica', 'Arial', sans-serif",
  },
  palette: {
    primary: {
      main: '#0D47A1',
    },
    secondary: {
      main: '#FF4081',
    },
    background: {
      default: '#F5F5F5',
      paper: '#FFFFFF',
    },
    text: {
      primary: '#212121',
      secondary: '#757575',
    },
    success: {
      main: '#4CAF50',
    },
    error: {
      main: '#F44336',
    },
    warning: {
      main: '#FF9800',
    },
    info: {
      main: '#2196F3',
    },
  },
  components: {
    MuiPaper: {
      styleOverrides: {
        root: {
          boxShadow: '0 4px 8px rgba(0, 0, 0, 0.03)',
          borderRadius: '16px',
          border: '1px solid rgba(0, 0, 0, 0.12)',
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: 'rgb(255 255 255 / 0.7)',
          borderBottom: '1px solid #D3D3D3',
          color: '#212121',
          boxShadow: '0 4px 8px rgba(0, 0, 0, 0.03)',
        },
      },
    },
    MuiSelect: {
      styleOverrides: {
        root: {
          borderRadius: '16px',
        },
      },
    },
    MuiFormControl: {
      styleOverrides: {
        root: {
          borderRadius: '16px',
        },
      },
    },
    MuiToggleButton: {
      styleOverrides: {
        root: {
          borderRadius: '16px',
          boxShadow: '0 4px 8px rgba(0, 0, 0, 0.012)',
        },
      },
    },
    MuiDateTimePicker: {
      styleOverrides: {
        root: {
          borderRadius: '16px',
          boxShadow: '0 4px 8px rgba(0, 0, 0, 0.03)',
        },
      },
    },
  },
});

export default theme;

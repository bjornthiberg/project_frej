import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import GitHubIcon from '@mui/icons-material/GitHub';
import Box from '@mui/material/Box';
import Logo from '../assets/images/header.png';

const Navbar = () => {
  return (
    <AppBar position="static" sx={{ borderRadius: 0 }}>
      <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Link href="/" sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none', color: 'inherit' }}>
          <Box component="img" src={Logo} alt="Logo" sx={{ height: 40 }} />
        </Link>
        <Box sx={{ textAlign: 'center' }}>
          <Typography variant="h5" sx={{ fontWeight: 'bold' }}>
            Project Frej Dashboard
          </Typography>
          <Typography variant="subtitle2" sx={{ color: '#5c5c5c' }}>
            RPi Sensor Data Monitoring
          </Typography>
        </Box>
        <Link href="https://github.com/bjornthiberg/project_frej" target="_blank" rel="noopener" sx={{ color: 'inherit' }}>
          <GitHubIcon sx={{ fontSize: 30 }} />
        </Link>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;

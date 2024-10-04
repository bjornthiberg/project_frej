import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import GitHubIcon from '@mui/icons-material/GitHub';
import Box from '@mui/material/Box';
import HomeIcon from '@mui/icons-material/Home';

const Navbar = () => {
  return (
    <AppBar position="static" sx={{ borderRadius: 0 }}>
      <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Link href="/" sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none', color: 'inherit' }}>
          <HomeIcon sx={{
            color: '#5c5c5c',
            fontSize: 30,
            transition: 'transform 0.3s ease-in-out',
            '&:hover': {
              transform: 'scale(1.1)',
            }
          }} />
        </Link>
        <Box sx={{ textAlign: 'center' }}>
          <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
            Project Frej
          </Typography>
          <Typography variant="subtitle2" sx={{ color: 'gray' }}>
            Environmental data from my apartment 
          </Typography>
        </Box>
        <Link href="https://github.com/bjornthiberg/project_frej" target="_blank" rel="noopener" sx={{ color: 'inherit' }}>
          <GitHubIcon sx={{
            color: '#5c5c5c',
            fontSize: 30,
            transition: 'transform 0.3s ease-in-out',
            '&:hover': {
              transform: 'scale(1.1)',
            }
          }} />
        </Link>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;

import { AppBar, Box, Button, Container, Toolbar, Typography } from "@mui/material";

const Header: React.FC = () => {
    return (
        <AppBar position='static'>
            <Container maxWidth='lg'>
                <Toolbar disableGutters>
                    <Typography
                        variant="h4"
                        noWrap
                        component="a"
                        href="/"
                        sx={{
                            mr: 2,
                            display: { xs: 'none', md: 'flex' },
                            fontFamily: 'monospace',
                            fontWeight: 700,
                            letterSpacing: '.3rem',
                            color: 'inherit',
                            textDecoration: 'none',
                        }}
                    >
                        Shrink Link
                    </Typography>
                    <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
                        <Button
                            href='/login'
                            sx={{ my: 2, color: 'white', display: 'block' }}
                        >
                            Sign In
                        </Button>
                        <Button
                            href='/register'
                            sx={{ my: 2, color: 'white', display: 'block' }}
                        >
                            Sign Up
                        </Button>
                    </Box>
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Header;
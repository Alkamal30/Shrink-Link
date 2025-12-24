import { AppBar, Box, Button, Container, Toolbar, Typography } from "@mui/material";
import { useAuth } from "./AuthProvider";

const Header: React.FC = () => {
    const auth = useAuth();

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
                        {
                            auth.isAuthenticated ? (
                                <>
                                    <Typography sx={{
                                        color: "white",
                                        display: "flex",
                                        alignItems: "center",
                                    }}>
                                        {auth.user?.name ?? auth.user?.email ?? auth.user?.sub}
                                    </Typography>

                                    <Button
                                        onClick={auth.signOut}
                                        sx={{ my: 2, color: "white", display: "block" }}
                                    >
                                        Sign Out
                                    </Button>
                                </>
                            ) : (
                                <>
                                    <Button
                                        onClick={auth.signIn}
                                        sx={{ my: 2, color: 'white', display: 'block' }}
                                    >
                                        Sign In
                                    </Button>
                                    {/* <Button
                                        sx={{ my: 2, color: 'white', display: 'block' }}
                                    >
                                        Sign Up
                                    </Button> */}
                                </>
                            )
                        }
                    </Box>
                </Toolbar>
            </Container>
        </AppBar>
    );
};

export default Header;
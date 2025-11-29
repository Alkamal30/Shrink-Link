import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Card, Divider, FormControl, FormLabel, TextField, Typography, Link } from '@mui/material';
import { FacebookIcon, GoogleIcon } from './CustomIcons';

const Login: React.FC = () => {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        // Здесь будет логика отправки данных на сервер
        console.log('Login Data:', { email, password });

        // Имитация успешного входа и редирект на главную
        alert('Вход выполнен!');
        navigate('/');
    };

    return (
        <Card variant='outlined' sx={{ p: 4, mx: 2, width: '100%', maxWidth: 400 }}>
            <Box sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: 3
            }}>
                <Typography
                    component="h1"
                    variant="h4"
                    sx={{ width: '100%', mb: 1, fontSize: 'clamp(2rem, 10vw, 2.15rem)' }}
                >
                    Sign In
                </Typography>
                <Box
                    component="form"
                    onSubmit={handleSubmit}
                    noValidate
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        width: '100%',
                        gap: 3,
                    }}
                >
                    <FormControl>
                        <FormLabel htmlFor="email">Email</FormLabel>
                        <TextField
                            // error={emailError}
                            // helperText={emailErrorMessage}
                            id="email"
                            type="email"
                            name="email"
                            placeholder="your@email.com"
                            autoComplete="email"
                            autoFocus
                            required
                            fullWidth
                            variant="outlined"
                            // color={emailError ? 'error' : 'primary'}
                            color='primary'
                        />
                    </FormControl>
                    <FormControl>
                        <FormLabel htmlFor="password">Password</FormLabel>
                        <TextField
                            // error={passwordError}
                            // helperText={passwordErrorMessage}
                            name="password"
                            placeholder="••••••"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            autoFocus
                            required
                            fullWidth
                            variant="outlined"
                            // color={passwordError ? 'error' : 'primary'}
                            color='primary'
                        />
                    </FormControl>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                    >
                        Sign In
                    </Button>
                </Box>
                <Divider>or</Divider>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <Button
                        fullWidth
                        variant="outlined"
                        onClick={() => alert('Sign in with Google')}
                        startIcon={<GoogleIcon />}
                    >
                        Sign in with Google
                    </Button>
                    <Button
                        fullWidth
                        variant="outlined"
                        onClick={() => alert('Sign in with Facebook')}
                        startIcon={<FacebookIcon />}
                    >
                        Sign in with Facebook
                    </Button>
                    <Typography sx={{ textAlign: 'center' }}>
                        Don&apos;t have an account?{' '}
                        <Link
                            href="/material-ui/getting-started/templates/sign-in/"
                            variant="body2"
                            sx={{ alignSelf: 'center' }}
                        >
                            Sign up
                        </Link>
                    </Typography>
                </Box>
            </Box>
        </Card>
    );
};

export default Login;
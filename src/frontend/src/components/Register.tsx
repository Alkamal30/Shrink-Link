import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Card, Divider, FormControl, FormLabel, TextField, Typography, Link } from '@mui/material';
import { FacebookIcon, GoogleIcon } from './CustomIcons';

const Register: React.FC = () => {
    const [name, setName] = useState<string>('');
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        // Здесь будет логика регистрации
        console.log('Register Data:', { name, email, password });

        alert('Регистрация успешна!');
        navigate('/login');
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
                    Sign Up
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
                        Sign Up
                    </Button>
                </Box>
                <Divider>or</Divider>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <Button
                        fullWidth
                        variant="outlined"
                        onClick={() => alert('Sign up with Google')}
                        startIcon={<GoogleIcon />}
                    >
                        Sign up with Google
                    </Button>
                    <Button
                        fullWidth
                        variant="outlined"
                        onClick={() => alert('Sign up with Facebook')}
                        startIcon={<FacebookIcon />}
                    >
                        Sign up with Facebook
                    </Button>
                    <Typography sx={{ textAlign: 'center' }}>
                        Already have an account?{' '}
                        <Link
                            href="/login"
                            variant="body2"
                            sx={{ alignSelf: 'center' }}
                        >
                            Sign in
                        </Link>
                    </Typography>
                </Box>
            </Box>
        </Card>
    );
};

export default Register;
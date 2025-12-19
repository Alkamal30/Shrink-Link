import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Card, Divider, FormControl, FormLabel, TextField, Typography, Link, Alert } from '@mui/material';
import { FacebookIcon, GoogleIcon } from './CustomIcons';

const Register: React.FC = () => {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setIsSubmitting(true);

        try {
            const res = await fetch('http://localhost:5001/api/Register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password }),
            });

            if (!res.ok) {
                const ct = res.headers.get('content-type') || '';
                let message = 'Ошибка регистрации';

                if (ct.includes('application/json')) {
                    const data = await res.json().catch(() => null);
                    message =
                        data?.message ||
                        data?.title ||
                        (typeof data === 'string' ? data : JSON.stringify(data)) ||
                        message;
                } else {
                    const text = await res.text().catch(() => '');
                    if (text) message = text;
                }

                throw new Error(message);
            }

            navigate('/login');
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Ошибка регистрации');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Card variant="outlined" sx={{ p: 4, mx: 2, width: '100%', maxWidth: 400 }}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
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
                    sx={{ display: 'flex', flexDirection: 'column', width: '100%', gap: 3 }}
                >
                    {error && <Alert severity="error">{error}</Alert>}

                    <FormControl>
                        <FormLabel htmlFor="email">Email</FormLabel>
                        <TextField
                            id="email"
                            type="email"
                            name="email"
                            placeholder="your@email.com"
                            autoComplete="email"
                            required
                            fullWidth
                            variant="outlined"
                            color="primary"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </FormControl>

                    <FormControl>
                        <FormLabel htmlFor="password">Password</FormLabel>
                        <TextField
                            name="password"
                            placeholder="••••••"
                            type="password"
                            id="password"
                            autoComplete="new-password"
                            required
                            fullWidth
                            variant="outlined"
                            color="primary"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </FormControl>

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? 'Signing up…' : 'Sign Up'}
                    </Button>
                </Box>

                <Divider>or</Divider>

                {/* остальное без изменений */}
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <Button
                        fullWidth
                        variant="outlined"
                        startIcon={<GoogleIcon />}
                        onClick={() => alert('Sign up with Google')
                    }>
                        Sign up with Google
                    </Button>
                    <Button
                        fullWidth
                        variant="outlined"
                        startIcon={<FacebookIcon />}
                        onClick={() => alert('Sign up with Facebook')                            
                    }>
                        Sign up with Facebook
                    </Button>
                    <Typography sx={{ textAlign: 'center' }}>
                        Already have an account?{' '}
                        <Link href="/login" variant="body2" sx={{ alignSelf: 'center' }}>
                            Sign in
                        </Link>
                    </Typography>
                </Box>
            </Box>
        </Card>
    );
};

export default Register;
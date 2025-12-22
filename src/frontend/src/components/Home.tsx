import { useCallback, useState } from "react";
import {
    Box,
    Button,
    CircularProgress,
    Link,
    TextField,
    Typography,
    Alert,
} from "@mui/material";
import { shrinkLink } from "../services/linkService";
import { useAuth } from "react-oidc-context";

const Home: React.FC = () => {
    const [value, setValue] = useState("");
    const [result, setResult] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    // const auth = useAuth();

    const handleClick = useCallback(async () => {
        const trimmedValue = value.trim();

        if (!trimmedValue) {
            setError("Input field is empty!");
            return;
        }

        setLoading(true);
        setError("");
        setResult("");

        try {
            const shrinkedLink = await shrinkLink(trimmedValue);
            setResult(shrinkedLink);
        } catch (err: unknown) {
            setError(err instanceof Error ? err.message : "Unknown error");
        } finally {
            setLoading(false);
        }
    }, [value]);

    const checkAuth = async () => {
        const res = await fetch('http://localhost:5001/auth/me', {
            method: 'GET'
        });

        const resJson = await res.json();

        console.log(resJson);
        alert(resJson.isAuthenticated ? 'Authenticated' : 'Not authenticated');
    };

    const handleLogin = async () => {
        window.location.assign('http://localhost:5001/auth/SignIn');
    };

    const handleLogOut = async () => {
        const res = await fetch('http://localhost:5001/auth/LogOut', {
            method: 'POST'
        });

        alert(res.ok ? 'Logged out' : 'Logout failed');
    };

    return (
        <Box
            sx={{
                width: "100%",
                maxWidth: 480,
                display: "flex",
                flexDirection: "column",
                gap: 3,
            }}
        >
            {/* Заголовок */}
            <Box textAlign="center">
                <Typography variant="h2" fontWeight={600}>
                    Shrink Link
                </Typography>
                <Typography color="text.secondary">
                    Shrink link — in a blink!
                </Typography>
            </Box>

            <div style={{display: "flex", flexDirection: "column", gap: "8px"}}>
                {/* {!auth.isAuthenticated ? (
                    <button onClick={() => auth.signinRedirect()}>Login</button>
                ) : (
                    <>
                        <div>Hello, {auth.user?.profile?.name ?? "user"}</div>
                        <button onClick={() => auth.signoutSilent()}>Logout</button>
                    </>
                )} */}

                <button onClick={() => handleLogin()}>Login</button>
                <button onClick={() => handleLogOut()}>Log Out</button>
            </div>

            <div>
                <button onClick={() => checkAuth()}>Check</button>
            </div>

            {/* Инпут и кнопка */}
            <Box display="flex" flexDirection="column" gap={2}>
                <TextField
                    label="Your link"
                    placeholder="www.your-link.com"
                    value={value}
                    onChange={(e) => setValue(e.target.value)}
                    fullWidth
                />

                <Button
                    variant="contained"
                    size="large"
                    onClick={handleClick}
                    disabled={loading || !value.trim()}
                >
                    {loading ? <CircularProgress size={24} /> : "Shrink"}
                </Button>

                <Typography variant="body2" color="text.secondary" textAlign="center">
                    Enter your link and click button to shorten
                </Typography>
            </Box>

            {/* Результат */}
            {result && (
                <Alert severity="success">
                    <Link href={result} target="_blank" rel="noopener">
                        {result}
                    </Link>
                </Alert>
            )}

            {/* Ошибка */}
            {error && <Alert severity="error">{error}</Alert>}
        </Box>
    );
};

export default Home;
import { useEffect } from "react";
import { useAuth } from "react-oidc-context";

export default function AuthCallback() {
    const auth = useAuth();

    useEffect(() => {
        console.log("Processing authentication callback...");
    }, []);

    if (auth.isLoading) return <div>Signing in...</div>
    if (auth.error) return <div>Authentication Error: {auth.error.message}</div>
    if (auth.isAuthenticated) {
        window.location.href = "/"; // replace to navigate
        return null
    }

    return <div>Completing sign-in...</div>;
}
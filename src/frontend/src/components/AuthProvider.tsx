import React, { createContext, useContext, useEffect, useMemo, useState } from "react";

const BFF_HOST = import.meta.env.VITE_API_HOST as string;

type User = {
    isAuthenticated: boolean;
    sub: string;
    name?: string;
    email?: string;
    roles?: string[];
};

type AuthContextValue = {
    user: User | null;
    isAuthenticated: boolean;
    loading: boolean;
    me: () => Promise<User | null>;
    signIn: () => void;
    signOut: () => void;
};

const AuthContext = createContext<AuthContextValue | null>(null);

async function fetchJson<T>(url: string, init?: RequestInit): Promise<T> {
    const res = await fetch(url, {
        ...init,
        credentials: "include",
        headers: {
            ...(init?.headers ?? {}),
            Accept: "application/json",
        },
    });

    if (res.status === 401) throw new Error("unauthorized");
    if (!res.ok) throw new Error(`http_${res.status}`);

    return (await res.json()) as T;
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);

    const me = async () => {
        try {
            const u = await fetchJson<User>(new URL('/Auth/Me', BFF_HOST).toString());
            setUser(u);
            return u;
        } catch {
            setUser(null);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const signIn = () => {
        window.location.assign(new URL('/Auth/SignIn', BFF_HOST).toString());
    };

    const signOut = () => {
        window.location.assign(new URL('/Auth/SignOut', BFF_HOST).toString());
    };

    useEffect(() => {
        void me();
    }, []);

    const value = useMemo<AuthContextValue>(
        () => ({
            user,
            isAuthenticated: user?.isAuthenticated === true,
            loading,
            me,
            signIn,
            signOut,
        }),
        [user, loading]
    );

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used within <AuthProvider />");
    return ctx;
}
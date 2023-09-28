import React, { useEffect, useState } from "react";
import { postSignIn, postSignOut } from "./Mutations";
import { getIsAuthenticated } from "./Queries";

export type AuthContextType = {
    isAuthenticated: boolean;
    hasFetchedAuthStatus: boolean;
    signIn: (credentials: SignInCredentials) => Promise<boolean>;
    signOut: () => Promise<boolean>;
};

export type SignInCredentials = {
    username: string;
    password: string;
};

const AuthContext = React.createContext<AuthContextType>({
    isAuthenticated: false,
    hasFetchedAuthStatus: false,
    signIn: async () => { return false },
    signOut: async () => { return false },
});

interface Props {
    children: React.ReactNode;
}

export const AuthContextProvider: React.FC<Props> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [hasFetchedAuthStatus, setHasFetchedAuthStatus] = useState(false);

    useEffect(() => {
        getIsAuthenticated().then((res) => {
            setIsAuthenticated(res);
            setHasFetchedAuthStatus(true);
        });
    }, []);

    const signInHandler = (credentials: SignInCredentials) => {
        return postSignIn(credentials)
            .then(res => {
                setIsAuthenticated(res.isSuccessful);
                return res.isSuccessful;
            });
    };

    const signOutHandler = () => {
        return postSignOut()
            .then(res => {
                if (res.isSuccessful)
                {
                    setIsAuthenticated(false);
                    return true;
                }
                return false;
            });
    };

    const contextValue: AuthContextType = {
        isAuthenticated,
        hasFetchedAuthStatus,
        signIn: signInHandler,
        signOut: signOutHandler,
    };

    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;

import React, { useState } from "react";
import { useSignInMutation, useSignOutMutation } from "./Mutations";
import { useIsAuthenticatedQuery } from "./Queries";
import { useNavigate } from "react-router-dom";

export type AuthContextType = {
    isAuthenticated: boolean;
    signIn: (credentials: SignInCredentials) => void;
    signOut: () => void;
};

export type SignInCredentials = {
    username: string;
    password: string;
};

const AuthContext = React.createContext<AuthContextType>({
    isAuthenticated: false,
    signIn: (__credentials) => {},
    signOut: () => {}
});

interface Props {
    children: React.ReactNode;
}

export const AuthContextProvider: React.FC<Props> = ({ children }) => {
    const navigate = useNavigate();
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    const signInMutation = useSignInMutation();
    const signOutMutation = useSignOutMutation();

    const signInHandler = (credentials: SignInCredentials) => {
        signInMutation.mutateAsync(credentials)
            .then(res => {
                if (res.isSuccessful)
                {
                    setIsAuthenticated(true);
                    navigate("/import-requests")
                }
            });
    };

    const signOutHandler = () => {
        signOutMutation.mutateAsync()
            .then(res => {
                if (res.isSuccessful)
                {
                    setIsAuthenticated(false);
                }
            });
    };

    const isAuthenticatedQuery = useIsAuthenticatedQuery();

    const contextValue: AuthContextType = {
        isAuthenticated: isAuthenticated || isAuthenticatedQuery.data || false,
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

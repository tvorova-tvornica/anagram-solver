import React, { useState } from "react";
import { useSignInMutation, useSignOutMutation } from "./Mutations";
import { useIsAuthenticatedQuery } from "./Queries";
import { useNavigate } from "react-router-dom";

export type AuthContextType = {
    isAuthenticated: boolean;
    signIn: (credentials: SignInCredentials) => void;
    hasInvalidSignInAttempt: boolean;
    signOut: () => void;
};

export type SignInCredentials = {
    username: string;
    password: string;
};

const AuthContext = React.createContext<AuthContextType>({
    isAuthenticated: false,
    signIn: (__credentials) => {},
    hasInvalidSignInAttempt: false,
    signOut: () => {}
});

interface Props {
    children: React.ReactNode;
}

export const AuthContextProvider: React.FC<Props> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [hasInvalidSignInAttempt, setHasInvalidSignInAttempt] = useState<boolean>(false);

    const signInMutation = useSignInMutation();
    const signOutMutation = useSignOutMutation();
    
    const navigate = useNavigate();

    const signInHandler = (credentials: SignInCredentials) => {
        signInMutation.mutateAsync(credentials)
            .then(res => {
                if (res.isSuccessful)
                {
                    setIsAuthenticated(true);
                    navigate("/import-requests");
                } else {
                    setHasInvalidSignInAttempt(true);
                }
            });
    };

    const signOutHandler = () => {
        signOutMutation.mutateAsync()
            .then(res => {
                if (res.isSuccessful)
                {
                    setIsAuthenticated(false);
                    navigate("/sign-in");
                }
            });
    };

    const isAuthenticatedQuery = useIsAuthenticatedQuery();

    const contextValue: AuthContextType = {
        isAuthenticated: isAuthenticated || isAuthenticatedQuery.data || false,
        signIn: signInHandler,
        hasInvalidSignInAttempt,
        signOut: signOutHandler,
    };

    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;

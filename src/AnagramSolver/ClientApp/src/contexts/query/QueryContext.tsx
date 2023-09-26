import React, { useContext } from "react";
import { MutationCache, QueryCache, QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { UnauthorizedError } from "./UnauthorizedError";
import AuthContext from "../auth/AuthContext";
import { useNavigate } from "react-router-dom";

interface Props {
    children: React.ReactNode;
}

export const QueryContextProvider: React.FC<Props> = ({ children }) => {
    const authContext = useContext(AuthContext);
    const navigate = useNavigate();

    const handleUnauthorizedError = (error: any) => {
        if (error instanceof UnauthorizedError) {
            authContext.signOut()
                .then(__ => navigate("/sign-in"));
        }
    };

    const queryClient = new QueryClient({
        defaultOptions: {
            queries: {
                refetchOnMount: false,
                refetchOnWindowFocus: false,
            },
        },
        queryCache: new QueryCache({
            onError: handleUnauthorizedError,
        }),
        mutationCache: new MutationCache({
            onError: handleUnauthorizedError,
        }),
    });

    return (
        <QueryClientProvider client={queryClient}>
            {children}
        </QueryClientProvider>
    );
};

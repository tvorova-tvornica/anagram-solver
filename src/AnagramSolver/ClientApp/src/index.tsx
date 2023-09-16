import { ChakraProvider, ColorModeScript } from "@chakra-ui/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes } from "react-router-dom";
import reportWebVitals from "./reportWebVitals";
import { theme } from "./theme";
import { ToggleColorMode } from "./components/toggle-color-mode";
import { AuthContextProvider } from "./contexts/auth/AuthContext";
import { AppRoutes } from "./components/routes";

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            refetchOnMount: false,
            refetchOnWindowFocus: false,
        },
    },
});

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

root.render(
    <React.StrictMode>
        <ColorModeScript initialColorMode={theme.config.initialColorMode} />
        <QueryClientProvider client={queryClient}>
            <ChakraProvider theme={theme}>
                <ToggleColorMode />
                <BrowserRouter>
                    <AuthContextProvider>
                        <AppRoutes />
                    </AuthContextProvider>
                </BrowserRouter>
            </ChakraProvider>
        </QueryClientProvider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

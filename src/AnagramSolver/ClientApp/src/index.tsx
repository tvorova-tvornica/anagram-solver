import { ChakraProvider, ColorModeScript } from "@chakra-ui/react";
import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import reportWebVitals from "./reportWebVitals";
import { theme } from "./theme";
import { ToggleColorMode } from "./components/toggle-color-mode";
import { AuthContextProvider } from "./contexts/auth/AuthContext";
import { QueryContextProvider } from "./contexts/query/QueryContext";
import { AppRoutes } from "./components/routes";

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

root.render(
    <React.StrictMode>
        <AuthContextProvider>
            <ColorModeScript initialColorMode={theme.config.initialColorMode} />
            <QueryContextProvider>
                <ChakraProvider theme={theme}>
                    <ToggleColorMode />
                    <BrowserRouter>
                        <AppRoutes />
                    </BrowserRouter>
                </ChakraProvider>
            </QueryContextProvider>
        </AuthContextProvider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

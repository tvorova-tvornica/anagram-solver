import { ChakraProvider } from "@chakra-ui/react";
import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { SiteLayout } from "./components/site-layout";
import { Home } from "./pages/Home";
import reportWebVitals from "./reportWebVitals";

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

root.render(
    <React.StrictMode>
        <ChakraProvider>
            <BrowserRouter>
                <SiteLayout>
                    <Routes>
                        <Route path="/" Component={Home} />
                    </Routes>
                </SiteLayout>
            </BrowserRouter>
        </ChakraProvider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

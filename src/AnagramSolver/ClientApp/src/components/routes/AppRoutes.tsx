import { useContext } from "react";
import { FC } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "../../pages/Home";
import { SignIn } from "../../pages/SignIn";
import { CelebritiesImport } from "../../pages/CelebritiesImport";
import AuthContext from "../../contexts/auth/AuthContext";

export const AppRoutes: FC<{}> = () => {
    const authCtx = useContext(AuthContext);

    return (
        <Routes>
            <Route path="/" Component={Home} />
            {!authCtx.isAuthenticated && (
                <Route path="/sign-in" Component={SignIn} />
            )}
            {authCtx.isAuthenticated && (
                <Route path="/import-requests" Component={CelebritiesImport} />
            )}
        </Routes>
    );
};

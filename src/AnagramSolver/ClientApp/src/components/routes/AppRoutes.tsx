import { FC } from "react";
import { Route, Routes } from "react-router-dom";
import { Home } from "../../pages/Home";
import { SignIn } from "../../pages/SignIn";
import { CelebritiesImport } from "../../pages/CelebritiesImport";
import { RequireAuth } from "./RequireAuth";

export const AppRoutes: FC<{}> = () => {
    return (
        <Routes>
            <Route path="/" Component={Home} />
            <Route path="/sign-in" Component={SignIn} />
            <Route 
                path="/import-requests"
                element={
                    <RequireAuth>
                        <CelebritiesImport />
                    </RequireAuth>
                }>
            </Route>
        </Routes>
    );
};

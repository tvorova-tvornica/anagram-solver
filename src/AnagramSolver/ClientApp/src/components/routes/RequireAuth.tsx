import React, { useContext } from "react";
import AuthContext from "../../contexts/auth/AuthContext";
import {
    Navigate,
    useLocation,
} from "react-router-dom";

interface Props {
    children: JSX.Element;
}

export const RequireAuth: React.FC<Props> = ({ children }) => {
    const authContext = useContext(AuthContext);
    const location = useLocation();

    if (!authContext.isAuthenticated) {
        return <Navigate to="/sign-in" state={{ from: location }} replace />;
    }

    return children;
}

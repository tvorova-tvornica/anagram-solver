import { FC, PropsWithChildren } from "react";
import { Navbar } from "./navbar/Navbar";

export const SiteLayout: FC<PropsWithChildren> = ({
    children,
}): JSX.Element => {
    return (
        <>
            <Navbar />
            {children}
        </>
    );
};

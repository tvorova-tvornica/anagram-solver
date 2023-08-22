import { Link } from "@chakra-ui/react";
import { FC } from "react";
import { Link as RouterLink } from "react-router-dom";

type NavbarLinkProps = {
    title: string;
    path: string;
};

export const NavbarLink: FC<NavbarLinkProps> = ({ title, path }) => (
    <Link p="10px" as={RouterLink} to={path}>
        {title}
    </Link>
);

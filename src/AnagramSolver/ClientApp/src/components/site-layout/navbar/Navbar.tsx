import { Box, Stack } from "@chakra-ui/react";
import { NavbarLink } from "./NavbarLink";

export const Navbar = () => {
    return (
        <Box w="100%" minHeight="50px" background="ghostwhite">
            <Stack direction="row">
                <NavbarLink title="Anagram Solver" path="/" />
            </Stack>
        </Box>
    );
};

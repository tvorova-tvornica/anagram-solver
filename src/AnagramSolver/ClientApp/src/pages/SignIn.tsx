import { FC } from "react";

import { LogInForm } from "../components/auth";
import { Flex, useColorModeValue } from "@chakra-ui/react";

export const SignIn: FC<{}> = () => {
    
    return (
        <Flex
            minH={"100vh"}
            align={"center"}
            justify={"center"}
            bg={useColorModeValue("gray.50", "gray.800")}
        >
            <LogInForm />
        </Flex>
    );
};

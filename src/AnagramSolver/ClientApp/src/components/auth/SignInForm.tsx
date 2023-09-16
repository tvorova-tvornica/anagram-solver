import { FC, useContext, useState } from "react";

import {
    Input,
    Box,
    Button,
    FormControl,
    FormLabel,
    useColorModeValue,
    Stack,
    Heading,
    FormErrorMessage,
} from "@chakra-ui/react";

import AuthContext from "../../contexts/auth/AuthContext";

export const SignInForm: FC<{}> = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const authCtx = useContext(AuthContext);

    return (
        <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
            <Stack align={"center"}>
                <Heading fontSize={"3xl"}>Sign in to admin account</Heading>
            </Stack>
            <Box
                rounded={"lg"}
                bg={useColorModeValue("white", "gray.700")}
                boxShadow={"lg"}
                p={8}
            >
                <Stack spacing={4}>
                    <FormControl id="username">
                        <FormLabel>Username</FormLabel>
                        <Input
                            type="text"
                            value={username}
                            onChange={(event) =>
                                setUsername(event.target.value)
                            }
                            placeholder="Enter username"
                            id="username"
                            isInvalid={authCtx.hasInvalidSignInAttempt}
                        />
                    </FormControl>
                    <FormControl id="password" isInvalid={authCtx.hasInvalidSignInAttempt}>
                        <FormLabel>Password</FormLabel>
                        <Input
                            type="password"
                            value={password}
                            onChange={(event) =>
                                setPassword(event.target.value)
                            }
                            placeholder="Enter password"
                            id="password"
                            isInvalid={authCtx.hasInvalidSignInAttempt}
                        />
                        <FormErrorMessage>Invalid credentials</FormErrorMessage>
                    </FormControl>
                    <Stack spacing={10}>
                        <Button
                            bg={"blue.400"}
                            color={"white"}
                            _hover={{
                                bg: "blue.500",
                            }}
                            onClick={() => authCtx.signIn({ username, password })}
                        >
                            Sign in
                        </Button>
                    </Stack>
                </Stack>
            </Box>
        </Stack>
    );
};

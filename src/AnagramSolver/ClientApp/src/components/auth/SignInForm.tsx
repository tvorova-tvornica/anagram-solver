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
    Spinner,
} from "@chakra-ui/react";

import AuthContext from "../../contexts/auth/AuthContext";
import { Navigate, useLocation, useNavigate } from "react-router-dom";

export const SignInForm: FC<{}> = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [hasInvalidSignInAttempt, setHasInvalidSignInAttempt] = useState(false);
    const authCtx = useContext(AuthContext);
    const navigate = useNavigate();
    const location = useLocation();
    const colorModeValue = useColorModeValue("white", "gray.700");

    const from = location.state?.from?.pathname || "/import-requests";

    if (!authCtx.hasFetchedAuthStatus) {
        <Box>
            <Spinner></Spinner>
        </Box>;
    }

    if (authCtx.isAuthenticated) {
        return <Navigate to={from} />;
    }

    return (
        <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
            <Stack align={"center"}>
                <Heading fontSize={"3xl"}>You must log in</Heading>
            </Stack>
            <Box
                rounded={"lg"}
                bg={colorModeValue}
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
                            isInvalid={hasInvalidSignInAttempt}
                        />
                    </FormControl>
                    <FormControl
                        id="password"
                        isInvalid={hasInvalidSignInAttempt}
                    >
                        <FormLabel>Password</FormLabel>
                        <Input
                            type="password"
                            value={password}
                            onChange={(event) =>
                                setPassword(event.target.value)
                            }
                            placeholder="Enter password"
                            id="password"
                            isInvalid={hasInvalidSignInAttempt}
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
                            onClick={() =>
                                authCtx.signIn({ username, password })
                                    .then(isSuccessful => {
                                        if (isSuccessful) {
                                            navigate(from, { replace: true });
                                        } else {
                                            setHasInvalidSignInAttempt(true);
                                        }
                                    })
                            }
                        >
                            Sign in
                        </Button>
                    </Stack>
                </Stack>
            </Box>
        </Stack>
    );
};

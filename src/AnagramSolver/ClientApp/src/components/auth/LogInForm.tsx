import { FC, useState } from "react";

import {
    Input,
    Box,
    Button,
    FormControl,
    FormLabel,
    useColorModeValue,
    Stack,
    Heading,
} from "@chakra-ui/react";

import { useLogInMutation } from "./Mutations";

export const LogInForm: FC<{}> = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const logInMutation = useLogInMutation({ username, password });

    return (
        <Stack spacing={8} mx={"auto"} maxW={"lg"} py={12} px={6}>
            <Stack align={"center"}>
                <Heading fontSize={"4xl"}>Sign in to your account</Heading>
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
                        />
                    </FormControl>
                    <FormControl id="password">
                        <FormLabel>Password</FormLabel>
                        <Input
                            type="password"
                            value={password}
                            onChange={(event) =>
                                setPassword(event.target.value)
                            }
                            placeholder="Enter password"
                        />
                    </FormControl>
                    <Stack spacing={10}>
                        <Button
                            bg={"blue.400"}
                            color={"white"}
                            _hover={{
                                bg: "blue.500",
                            }}
                            onClick={() => logInMutation.mutate()}
                        >
                            Sign in
                        </Button>
                    </Stack>
                </Stack>
            </Box>
        </Stack>
    );
};

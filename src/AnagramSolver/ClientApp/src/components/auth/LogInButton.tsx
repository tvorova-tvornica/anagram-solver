import { FC, useState } from "react";

import { Input, Button } from "@chakra-ui/react";

import { useLogInMutation } from "./Mutations";

export const LogInButton: FC<{}> = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const logInMutation = useLogInMutation({ username, password });

    return (
        <>
            <Input
                value={username}
                onChange={(event) => setUsername(event.target.value)}
                placeholder="Enter username"
            />
            <Input
                type={"password"}
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Enter password"
            />
            <Button onClick={() => logInMutation.mutate()}>Log In</Button>
        </>
    );
};

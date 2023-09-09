import { useMutation } from "@tanstack/react-query";

export const useLogInMutation = ({
    username,
    password,
}: {
    username: string;
    password: string;
}) =>
    useMutation({
        mutationKey: ["log-in", username, password],
        mutationFn: async () => {
            await fetch("/auth/log-in", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ username, password }),
            });
        },
    });

import { useMutation } from "@tanstack/react-query";
import { SignInCredentials } from "./AuthContext";

export const useSignInMutation = () =>
    useMutation({
        mutationKey: ["sign-in"],
        mutationFn: async (credentials: SignInCredentials) => {
            const result = await fetch("/auth/sign-in", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(credentials),
            });

            return {
                isSuccessful: result.status === 200
            };
        },
    });

export const useSignOutMutation = () =>
    useMutation({
        mutationKey: ["sign-out"],
        mutationFn: async () => {
            const result = await fetch("/auth/sign-out", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            return {
                isSuccessful: result.status === 200
            };
        },
    });

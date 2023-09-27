import { SignInCredentials } from "./AuthContext";

export const postSignIn = async (credentials: SignInCredentials) => {
    const result = await fetch("/auth/sign-in", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(credentials),
    });

    return {
        isSuccessful: result.status === 200,
    }
};

export const postSignOut = async () => {
    const result = await fetch("/auth/sign-out", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
    });

    return {
        isSuccessful: result.status === 200,
    }
};

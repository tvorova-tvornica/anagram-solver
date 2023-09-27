export const getIsAuthenticated = async () => {
    var response = await fetch("/auth/is-authenticated");
    return (await response.json()) as boolean;
};

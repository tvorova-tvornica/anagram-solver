import { useQuery } from "@tanstack/react-query";

export const useIsAuthenticatedQuery = () =>
    useQuery({
        queryKey: ["is-authenticated"],
        queryFn: async () => {
            const response = await fetch(
                `/auth/is-authenticated`
            );
            return (await response.json()) as boolean;
        }
    });

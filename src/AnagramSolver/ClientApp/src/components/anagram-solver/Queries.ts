import { useQuery } from "@tanstack/react-query";

export type SolveAnagramResult = {
    fullName: string;
    photoUrl?: string;
    description?: string;
    wikipediaUrl?: string;
};

export const useSolveAnagramQuery = (anagram: string) =>
    useQuery({
        queryKey: ["solve-anagram", anagram],
        enabled: !!anagram,
        queryFn: async () => {
            const response = await fetch(
                `/celebrity/solve-anagram?anagram=${anagram}`
            );
            return (await response.json()) as SolveAnagramResult[];
        },
    });

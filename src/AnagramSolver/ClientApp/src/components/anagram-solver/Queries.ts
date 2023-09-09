import { useQuery } from "@tanstack/react-query";

export type ResolveAnagramResult = {
    fullName: string;
    photoUrl?: string;
    wikipediaUrl?: string;
};

export const useResolveAnagramQuery = (anagram: string) =>
    useQuery({
        queryKey: ["resolve-anagram", anagram],
        enabled: !!anagram,
        queryFn: async () => {
            const response = await fetch(
                `/celebrity/resolve-anagram?anagram=${anagram}`
            );
            return (await response.json()) as ResolveAnagramResult[];
        },
    });

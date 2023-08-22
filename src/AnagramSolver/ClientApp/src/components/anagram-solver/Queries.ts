import { useQuery } from "@tanstack/react-query";

export const useResolveAnagramQuery = (anagram: string) => useQuery({
    queryKey: ['resolve-anagram', anagram],
    enabled: !!anagram,
    queryFn: async () => {
        const response = await fetch(`/celebrity/resolve-anagram?anagram=${anagram}`);
        return (await response.json()) as string[];
    }
})
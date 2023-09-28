import { useQuery } from "@tanstack/react-query";
import { UnauthorizedError } from "../../contexts/query/UnauthorizedError";

export type ImportCelebritiesRequestResult = {
    createdAt: string,
    wikiDataNationalityId?: string,
    wikiDataOccupationId?: string,
    completionPercentage: number,
};

export const useGetImportCelebritiesRequestsQuery = (page: number, pageSize: number) =>
    useQuery({
        queryKey: ["get-import-celebrities-requests", page, pageSize],
        enabled: !!page && !!pageSize,
        queryFn: async () => {
            const response = await fetch(
                `/celebrity/get-import-celebrities-requests?page=${page}&pageSize=${pageSize}`
            );

            if (response.status === 401) {
                throw new UnauthorizedError();
            }

            return (await response.json()) as ImportCelebritiesRequestResult[];
        }
    });

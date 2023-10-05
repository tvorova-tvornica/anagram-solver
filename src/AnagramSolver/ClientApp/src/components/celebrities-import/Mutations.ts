import { useMutation } from "@tanstack/react-query";
import { UnauthorizedError } from "../../contexts/query/UnauthorizedError";

export const useRequestCelebritiesImportMutation = (nationalityId?: string, occupationId?: string) =>
    useMutation({
        mutationKey: ["request-celebrities-import", nationalityId, occupationId],
        retry: false,
        mutationFn: async () => {
            const response = await fetch("/celebrities/request-celebrities-import", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ nationalityId, occupationId }),
            });

            if (response.status === 401) {
                throw new UnauthorizedError();
            }
        }
    });

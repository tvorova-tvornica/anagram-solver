import { Flex } from "@chakra-ui/react";
import { ImportCelebritiesRequestsTable } from "../components/celebrities-import/ImportCelebritiesRequestsTable";

export function CelebritiesImport() {
    return (
        <Flex w="100%" h="91vh" align="center" justify="center">
            <ImportCelebritiesRequestsTable/>
        </Flex>
    );
};

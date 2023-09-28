import { FC } from "react";
import { 
    Box, 
    Spinner, 
    Table, 
    TableCaption, 
    TableContainer, 
    Tbody, 
    Td, 
    Th, 
    Thead, 
    Tr 
} from "@chakra-ui/react";
import { useGetImportCelebritiesRequestsQuery } from "./Queries";

export const ImportCelebritiesRequestsTable: FC<{}> = () => {
    const importCelebritiesRequestsResult = useGetImportCelebritiesRequestsQuery(1, 10);

    if (importCelebritiesRequestsResult.isFetching)
    {
        return (
            <Box>
                <Spinner></Spinner>
            </Box>
        );
    }

    return (
        <Box w="95%" maxW="1800px">
            <TableContainer>
                <Table variant="simple" size={"sm"}>
                    <TableCaption>
                        Wiki data celebrities import requests
                    </TableCaption>
                    <Thead>
                        <Tr>
                            <Th>Created at</Th>
                            <Th>WikiData nationality Id</Th>
                            <Th>WikiData occupation Id</Th>
                            <Th>Completion percentage</Th>
                        </Tr>
                    </Thead>
                    <Tbody>
                        {importCelebritiesRequestsResult.data?.length &&
                            importCelebritiesRequestsResult.data.map((result, index) => (
                                <Tr key={index}>
                                    <Td>{result.createdAt}</Td>
                                    <Td>{result.wikiDataNationalityId}</Td>
                                    <Td>{result.wikiDataOccupationId}</Td>
                                    <Td>{result.completionPercentage.toFixed(2)}%</Td>
                                </Tr>
                        ))}
                    </Tbody>
                </Table>
            </TableContainer>
        </Box>
    );
};

import { FC, useState } from "react";
import {
    Box,
    Button,
    FormControl,
    FormLabel,
    HStack,
    Input,
    Spinner,
    Table,
    TableCaption,
    TableContainer,
    Tbody,
    Td,
    Th,
    Thead,
    Tr,
} from "@chakra-ui/react";
import { useGetImportCelebritiesRequestsQuery } from "./Queries";
import { useRequestCelebritiesImportMutation } from "./Mutations";

export const ImportCelebritiesRequestsTable: FC<{}> = () => {
    const [wikiDataNationalityId, setWikiDataNationalityId] = useState("");
    const [wikiDataOccupationId, setWikiDataOccupationId] = useState("");

    const importCelebritiesRequestsResult =
        useGetImportCelebritiesRequestsQuery(1, 10);
    const requestCelebritiesImportMutation =
        useRequestCelebritiesImportMutation(
            wikiDataNationalityId,
            wikiDataOccupationId
        );

    if (importCelebritiesRequestsResult.isFetching) {
        return (
            <Box>
                <Spinner></Spinner>
            </Box>
        );
    }

    return (
        <Box w="95%" maxW="1800px">
            <HStack>
                <FormControl maxW={"200px"}>
                    <FormLabel>Nationality</FormLabel>
                    <Input
                        value={wikiDataNationalityId}
                        onChange={(e) =>
                            setWikiDataNationalityId(e.target.value)
                        }
                    />
                </FormControl>
                <FormControl maxW={"200px"}>
                    <FormLabel>Occupation</FormLabel>
                    <Input
                        value={wikiDataOccupationId}
                        onChange={(e) =>
                            setWikiDataOccupationId(e.target.value)
                        }
                    />
                </FormControl>
                <FormControl>
                    <Button
                        marginTop={"30px"}
                        onClick={() =>
                            requestCelebritiesImportMutation
                                .mutateAsync()
                                .then(() => {
                                    importCelebritiesRequestsResult.refetch();
                                    setWikiDataNationalityId("");
                                    setWikiDataOccupationId("");
                                })
                        }
                        disabled={requestCelebritiesImportMutation.isLoading}
                    >
                        Request import
                    </Button>
                </FormControl>
            </HStack>
            <TableContainer>
                <Table variant="simple" size={"sm"}>
                    <TableCaption>
                        Wiki data import celebrities requests
                    </TableCaption>
                    <Thead>
                        <Tr>
                            <Th>Created at</Th>
                            <Th>Nationality Id</Th>
                            <Th>Occupation Id</Th>
                            <Th>Progress</Th>
                        </Tr>
                    </Thead>
                    <Tbody>
                        {importCelebritiesRequestsResult.data?.length &&
                            importCelebritiesRequestsResult.data.map(
                                (result, index) => (
                                    <Tr key={index}>
                                        <Td>
                                            {new Date(
                                                result.createdAt
                                            ).toLocaleDateString()}
                                        </Td>
                                        <Td>{result.wikiDataNationalityId}</Td>
                                        <Td>{result.wikiDataOccupationId}</Td>
                                        <Td>
                                            {result.completionPercentage.toFixed(
                                                2
                                            )}
                                            %
                                        </Td>
                                    </Tr>
                                )
                            )}
                    </Tbody>
                </Table>
            </TableContainer>
        </Box>
    );
};

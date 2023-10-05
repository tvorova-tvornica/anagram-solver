import {
    Box,
    Card,
    CardBody,
    Heading,
    Input,
    InputGroup,
    InputRightElement,
    Spinner,
    Stack,
    Text,
    VStack,
} from "@chakra-ui/react";
import { AnimatePresence, motion } from "framer-motion";
import { FC, useState } from "react";
import { useDebounce } from "../../hooks/useDebounce";
import { AnimatedText } from "../animated-text/AnimatedText";
import { AnagramSolverResultImage } from "./AnagramSolverResultImage";
import { useSolveAnagramQuery } from "./Queries";

export const AnagramSolver: FC<{}> = () => {
    const [anagram, setAnagram] = useState("");
    const debouncedAnagram = useDebounce(anagram, 500);
    const solveAnagramResult = useSolveAnagramQuery(debouncedAnagram);

    return (
        <Box w="95%" maxW="600px">
            <AnimatedText
                m="auto"
                w="fit-content"
                fontSize={["40px", null, "55px"]}
                as="h1"
                text="Anagram Solver"
            />
            <Box
                as={motion.div}
                animate={{ y: 35 }}
                transition={{ delay: "1" }}
            >
                <InputGroup>
                    <Input
                        value={anagram}
                        onChange={(event) => setAnagram(event.target.value)}
                        padding="25px"
                        placeholder="nag a ram..."
                    />
                    <InputRightElement>
                        {solveAnagramResult.isFetching && (
                            <Spinner mt="10px" />
                        )}
                    </InputRightElement>
                </InputGroup>
            </Box>

            <VStack mt="50px" align="stretch">
                <AnimatePresence>
                    {solveAnagramResult.data?.length &&
                        solveAnagramResult.data.map((result, index) => (
                            <Card
                                cursor="pointer"
                                direction="row"
                                overflow="hidden"
                                initial={{ x: 50 }}
                                animate={{ x: 0 }}
                                exit={{ opacity: 0 }}
                                key={index}
                                as={motion.div}
                                onClick={() => {
                                    window.open(
                                        result?.wikipediaUrl ??
                                            `https://www.google.com/search?q=${result.fullName.replace(
                                                /\s/g,
                                                "+"
                                            )}`,
                                        "_blank"
                                    );
                                }}
                            >
                                <AnagramSolverResultImage
                                    imageUrl={result.photoUrl}
                                    alt={result.fullName}
                                />
                                <Stack>
                                    <CardBody>
                                        <Heading size="xl">
                                            {result.fullName}
                                        </Heading>
                                        {result.description && (
                                            <Text
                                                as="i"
                                                noOfLines={3}
                                                pl="6px"
                                                mt="6px"
                                                color="dimgrey"
                                                borderLeft="3px solid #69545f"
                                            >
                                                {result.description}
                                            </Text>
                                        )}
                                    </CardBody>
                                </Stack>
                            </Card>
                        ))}
                    {solveAnagramResult.data &&
                        !solveAnagramResult.data.length && (
                            <Card
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                key="not-found"
                                as={motion.div}
                            >
                                <CardBody>
                                    <Text color="red.500">
                                        We can't solve this one
                                    </Text>
                                </CardBody>
                            </Card>
                        )}
                </AnimatePresence>
            </VStack>
        </Box>
    );
};

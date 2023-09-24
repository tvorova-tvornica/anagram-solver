import {
    Box,
    Card,
    CardBody,
    Flex,
    Heading,
    Input,
    InputGroup,
    InputRightElement,
    Spinner,
    Stack,
    Text,
} from "@chakra-ui/react";
import { AnimatePresence, motion } from "framer-motion";
import { FC, useState } from "react";
import { useDebounce } from "../../hooks/useDebounce";
import { AnimatedText } from "../animated-text/AnimatedText";
import { useResolveAnagramQuery } from "./Queries";
import { AnagramSolverResultImage } from "./AnagramSolverResultImage";

export const AnagramSolver: FC<{}> = () => {
    const [anagram, setAnagram] = useState("");
    const debouncedAnagram = useDebounce(anagram, 500);
    const resolveAnagramResult = useResolveAnagramQuery(debouncedAnagram);

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
                        {resolveAnagramResult.isFetching && (
                            <Spinner mt="10px" />
                        )}
                    </InputRightElement>
                </InputGroup>
            </Box>

            <Flex direction="column" mt="50px" overflow="scroll">
                <AnimatePresence>
                    {resolveAnagramResult.data?.length &&
                        resolveAnagramResult.data.map((result, index) => (
                            <Card
                                cursor="pointer"
                                direction="row"
                                overflow="hidden"
                                m="1px"
                                mb="10px"
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
                    {resolveAnagramResult.data &&
                        !resolveAnagramResult.data.length && (
                            <Card
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                m="1px"
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
            </Flex>
            <Text pt={6} fontSize={"sm"} textAlign={"center"}>
                © {new Date().getFullYear()} PSEUDO_RASISTI
            </Text>
        </Box>
    );
};

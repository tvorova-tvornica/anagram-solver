import {
    Text,
    Box,
    Card,
    CardBody,
    Input,
    InputGroup,
    InputRightElement,
    Spinner,
    Flex,
} from "@chakra-ui/react";
import { AnimatePresence, motion } from "framer-motion";
import { FC, useState } from "react";
import { useDebounce } from "../../hooks/useDebounce";
import { AnimatedText } from "../animated-text/AnimatedText";
import { useResolveAnagramQuery } from "./Queries";

export const AnagramSolver: FC<{}> = () => {
    const [anagram, setAnagram] = useState("");
    const debouncedAnagram = useDebounce(anagram, 500);
    const resolveAnagramResult = useResolveAnagramQuery(debouncedAnagram);

    return (
        <Box minW={["300px", null, "600px"]} minH="500px" pt="5%">
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

            <Flex direction="column" mt="50px">
                <AnimatePresence>
                    {resolveAnagramResult.data?.length &&
                        resolveAnagramResult.data.map((resultTitle) => (
                            <Card
                                initial={{ x: 50 }}
                                animate={{ x: 0 }}
                                exit={{ opacity: 0 }}
                                key={resultTitle}
                                as={motion.div}
                            >
                                <CardBody>
                                    <Text>{resultTitle}</Text>
                                </CardBody>
                            </Card>
                        ))}
                    {resolveAnagramResult.data &&
                        !resolveAnagramResult.data.length && (
                            <Card
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
            </Flex>
        </Box>
    );
};

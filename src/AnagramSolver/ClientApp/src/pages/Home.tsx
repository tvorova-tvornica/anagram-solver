import { Flex } from "@chakra-ui/react";
import { AnagramSolver } from "../components/anagram-solver";

export function Home() {
    return (
        <Flex w="100%" h="91vh" align="center" justify="center">
            <AnagramSolver />
        </Flex>
    );
}

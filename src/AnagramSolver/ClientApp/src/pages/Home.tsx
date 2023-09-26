import { Flex } from "@chakra-ui/react";
import { AnagramSolver } from "../components/anagram-solver";

export function Home() {
    return (
        <Flex
            position="absolute"
            top={[60, null, null, null, 80]}
            w="100%"
            align="center"
            justify="center"
        >
            <AnagramSolver />
        </Flex>
    );
}

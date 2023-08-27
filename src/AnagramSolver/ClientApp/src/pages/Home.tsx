import { Flex } from "@chakra-ui/react";
import {ToggleColorMode} from '../components/toggle-color-mode';
import { AnagramSolver } from "../components/anagram-solver";

export function Home() {
    return (
        <Flex w="100%" h="91vh" align="center" justify="center">
            <ToggleColorMode/>
            <AnagramSolver />
        </Flex>
    );
}

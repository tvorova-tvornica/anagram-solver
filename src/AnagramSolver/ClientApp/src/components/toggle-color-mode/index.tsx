import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { Box, IconButton, useColorMode } from "@chakra-ui/react";

export function ToggleColorMode(): JSX.Element {
    const { colorMode, toggleColorMode } = useColorMode();

    return (
        <Box position="absolute" top="2%" right="1%">
            <IconButton
                aria-label="Toggle Mode"
                size='sm'
                variant="ghost"
                icon={colorMode === 'dark' ? <SunIcon /> : <MoonIcon />}
                onClick={toggleColorMode}
            />
        </Box>
    )
}
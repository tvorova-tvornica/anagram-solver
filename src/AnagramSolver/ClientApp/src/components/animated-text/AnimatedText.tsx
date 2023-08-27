import { Box, BoxProps } from "@chakra-ui/react";
import { motion } from "framer-motion";
import { FC } from "react";

type AnimatedTextProps = {
    text: string;
    as: keyof Pick<typeof motion, "a" | "div" | "span" | "h1" | "h2">;
} & Omit<BoxProps, "as">;

export const AnimatedText: FC<AnimatedTextProps> = ({
    as,
    text,
    ...styleProps
}) => {
    const words = text.split(" ").map((word) => [...word.split(""), "\u00A0"]);

    return (
        <Box
            as={motion.div}
            initial="hidden"
            animate="visible"
            variants={{
                visible: {
                    transition: {
                        staggerChildren: 0.025,
                    },
                },
            }}
        >
            <Box as={motion[as]} {...styleProps}>
                {words.map((word, index) => {
                    return (
                        <Box key={index} display="inline-block">
                            {word.flat().map((element, index) => {
                                return (
                                    <Box
                                        as="span"
                                        display="inline-block"
                                        overflow="hidden"
                                        key={index}
                                    >
                                        <Box
                                            as={motion.span}
                                            display="inline-block"
                                            variants={{
                                                hidden: {
                                                    y: "200%",
                                                    color: "#0055FF",
                                                    transition: {
                                                        ease: [
                                                            0.455, 0.03, 0.515,
                                                            0.955,
                                                        ],
                                                        duration: 0.85,
                                                    },
                                                },
                                                visible: {
                                                    y: 0,
                                                    color: "#69545f",
                                                    transition: {
                                                        ease: [
                                                            0.455, 0.03, 0.515,
                                                            0.955,
                                                        ],
                                                        duration: 0.75,
                                                    },
                                                },
                                            }}
                                        >
                                            {element}
                                        </Box>
                                    </Box>
                                );
                            })}
                        </Box>
                    );
                })}
            </Box>
        </Box>
    );
};

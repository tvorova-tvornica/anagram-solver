import {
    Flex,
    Icon,
    Image,
    Skeleton,
    useColorModeValue,
} from "@chakra-ui/react";
import React, { FC } from "react";
import { FiUser } from "react-icons/fi";

type AnagramSolverResultImageProps = {
    imageUrl?: string;
    alt?: string;
};

export const AnagramSolverResultImage: FC<AnagramSolverResultImageProps> = ({
    imageUrl,
    alt,
}) => {
    const placeholderBackgroundColor = useColorModeValue(
        "gray.200",
        "gray.600"
    );

    if (!imageUrl) {
        return (
            <Flex
                justify="center"
                align="center"
                background={placeholderBackgroundColor}
                minW={{
                    base: "100px",
                    sm: "120px",
                }}
                minH={{
                    base: "193px",
                    sm: "173px",
                }}
            >
                <Icon fontSize="50px" as={FiUser} />
            </Flex>
        );
    }

    return (
        <Image
            fit="cover"
            fallback={
                <Skeleton
                    w={{
                        base: "100px",
                        sm: "120px",
                    }}
                    h={{
                        base: "193px",
                        sm: "173px",
                    }}
                />
            }
            w={{ base: "100px", sm: "120px" }}
            h={{ base: "193px", sm: "173px" }}
            src={imageUrl}
            alt={alt}
        />
    );
};

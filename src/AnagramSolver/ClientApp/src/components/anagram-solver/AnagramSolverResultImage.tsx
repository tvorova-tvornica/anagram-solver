import {
    Flex,
    Icon,
    Image,
    LayoutProps,
    Skeleton,
    useColorModeValue,
} from "@chakra-ui/react";
import { FC } from "react";
import { FiUser } from "react-icons/fi";

type AnagramSolverResultImageProps = {
    imageUrl?: string;
    alt?: string;
};

const IMAGE_SIZING_VALUES: LayoutProps = {
    minW: {
        base: "100px",
        sm: "120px",
    },
    maxW: {
        base: "100px",
        sm: "120px",
    },
    minH: {
        base: "193px",
        sm: "173px",
    },
    maxH: {
        base: "193px",
        sm: "173px",
    },
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
                {...IMAGE_SIZING_VALUES}
            >
                <Icon fontSize="50px" as={FiUser} />
            </Flex>
        );
    }

    return (
        <Image
            fit="cover"
            fallback={<Skeleton {...IMAGE_SIZING_VALUES} />}
            {...IMAGE_SIZING_VALUES}
            src={imageUrl}
            alt={alt}
        />
    );
};

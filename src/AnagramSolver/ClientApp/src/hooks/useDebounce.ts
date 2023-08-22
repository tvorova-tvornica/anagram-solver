import { useState, useEffect } from "react";

export function useDebounce<T>(value: T, milliSeconds: number): T {
    const [debouncedValue, setDebouncedValue] = useState(value);

    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedValue(value);
        }, milliSeconds);

        return () => {
            clearTimeout(timer);
        };
    }, [value, milliSeconds]);

    return debouncedValue;
}

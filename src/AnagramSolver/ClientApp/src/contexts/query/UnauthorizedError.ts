export class UnauthorizedError extends Error {
    constructor() {
        super("Unauthorized request");

        Object.setPrototypeOf(this, UnauthorizedError.prototype);
    }
}

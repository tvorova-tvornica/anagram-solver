namespace AnagramSolver.Extensions;

public class InvalidFullNameException : Exception
{
    public InvalidFullNameException(string message) 
        : base(message)
    {
    }
}

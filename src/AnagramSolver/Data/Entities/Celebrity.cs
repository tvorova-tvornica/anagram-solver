namespace AnagramSolver.Data.Entities;

public class Celebrity 
{
    public int Id { get; private set; }
    public string FullName { get; init; }
    public string SortedName { get; init; }

    public Celebrity(string fullName)
    {
        FullName = fullName;
        SortedName = new string(fullName.Where(x => !char.IsWhiteSpace(x)).OrderBy(x => x).ToArray());
    }
}

namespace PermutationGenerator.Models;

public class StartResponse
{
    public Guid SessionId { get; set; }
    public string TotalPermutations { get; set; } = string.Empty;
}

namespace PermutationGenerator.Models;

public class NextResponse
{
    public int[] Permutation { get; set; } = Array.Empty<int>();
    public string Index { get; set; } = string.Empty;
    public bool HasMore { get; set; }
}

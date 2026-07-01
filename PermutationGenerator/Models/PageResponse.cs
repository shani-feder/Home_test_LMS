namespace PermutationGenerator.Models;

public class PageResponse
{
    public List<PermutationItem> Permutations { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string TotalItems { get; set; } = string.Empty;
    public string TotalPages { get; set; } = string.Empty;
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class PermutationItem
{
    public int[] Permutation { get; set; } = Array.Empty<int>();
    public string Index { get; set; } = string.Empty;
}

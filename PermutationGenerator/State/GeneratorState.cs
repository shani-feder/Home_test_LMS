using System.Numerics;

namespace PermutationGenerator.State;

public class GeneratorState
{
    public int N { get; set; }
    public int[] CurrentPermutation { get; set; } = Array.Empty<int>();
    public BigInteger CurrentIndex { get; set; }
    public BigInteger TotalPermutations { get; set; }
}

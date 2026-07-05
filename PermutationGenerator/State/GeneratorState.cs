using System.Numerics;

namespace PermutationGenerator.State;

public class GeneratorState
{
    public int N { get; init; }
    public int[] CurrentPermutation { get; set; } = Array.Empty<int>();
    public BigInteger CurrentIndex { get; set; }
    public BigInteger TotalPermutations { get; init; }

    // Protects the read→compute→write cycle per session against concurrent requests
    public SemaphoreSlim Semaphore { get; } = new(1, 1);
}

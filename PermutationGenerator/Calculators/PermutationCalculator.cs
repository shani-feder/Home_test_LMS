using System.Numerics;

namespace PermutationGenerator.Calculators;

public static class PermutationCalculator
{
    /// <summary>
    /// Computes n! (factorial). Supports up to 20! using BigInteger.
    /// </summary>
    public static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    /// <summary>
    /// Returns the first permutation [1, 2, ..., n].
    /// </summary>
    public static int[] GetFirstPermutation(int n)
    {
        var perm = new int[n];
        for (int i = 0; i < n; i++)
            perm[i] = i + 1;
        return perm;
    }

    /// <summary>
    /// Computes the next permutation in lexicographic order (in-place).
    /// Returns false if current is the last permutation.
    /// Algorithm: standard O(n) next permutation.
    /// </summary>
    public static bool NextPermutation(int[] perm)
    {
        int n = perm.Length;

        // Step 1: Find largest i such that perm[i] < perm[i+1]
        int i = n - 2;
        while (i >= 0 && perm[i] >= perm[i + 1])
            i--;

        // If no such i exists, this is the last permutation
        if (i < 0)
            return false;

        // Step 2: Find largest j such that perm[j] > perm[i]
        int j = n - 1;
        while (perm[j] <= perm[i])
            j--;

        // Step 3: Swap perm[i] and perm[j]
        (perm[i], perm[j]) = (perm[j], perm[i]);

        // Step 4: Reverse the suffix starting at perm[i+1]
        Array.Reverse(perm, i + 1, n - i - 1);

        return true;
    }

    /// <summary>
    /// Returns the permutation at a given 0-based index using Factoradic (Lehmer code).
    /// O(n²) complexity. Supports any index up to n!-1.
    /// </summary>
    public static int[] GetPermutationByIndex(int n, BigInteger index)
    {
        var result = new int[n];
        var available = new List<int>(n);

        for (int i = 1; i <= n; i++)
            available.Add(i);

        for (int i = 0; i < n; i++)
        {
            BigInteger fact = Factorial(n - 1 - i);
            int digitIndex = (int)(index / fact);
            result[i] = available[digitIndex];
            available.RemoveAt(digitIndex);
            index %= fact;
        }

        return result;
    }
}

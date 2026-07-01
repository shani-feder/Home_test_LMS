using System.Numerics;
using PermutationGenerator.Calculators;
using PermutationGenerator.Models;
using PermutationGenerator.State;

namespace PermutationGenerator.Services;

public class PermutationService
{
    private readonly IPermutationStateStore _stateStore;

    public PermutationService(IPermutationStateStore stateStore)
    {
        _stateStore = stateStore;
    }

    public async Task<StartResponse> StartAsync(int n)
    {
        if (n < 1 || n > 20)
            throw new ArgumentOutOfRangeException(nameof(n), "n must be between 1 and 20.");

        var sessionId = Guid.NewGuid();
        var total = PermutationCalculator.Factorial(n);

        var state = new GeneratorState
        {
            N = n,
            CurrentPermutation = Array.Empty<int>(),
            CurrentIndex = 0,
            TotalPermutations = total
        };

        await _stateStore.SaveAsync(sessionId, state);

        return new StartResponse
        {
            SessionId = sessionId,
            TotalPermutations = total.ToString("N0")
        };
    }

    public async Task<NextResponse> GetNextAsync(Guid sessionId)
    {
        var state = await _stateStore.GetAsync(sessionId)
            ?? throw new InvalidOperationException("Session not found. Call Start first.");

        // First call - return first permutation
        if (state.CurrentPermutation.Length == 0)
        {
            state.CurrentPermutation = PermutationCalculator.GetFirstPermutation(state.N);
            state.CurrentIndex = 1;
        }
        else
        {
            // Check if we've exhausted all permutations
            if (state.CurrentIndex >= state.TotalPermutations)
            {
                return new NextResponse
                {
                    Permutation = Array.Empty<int>(),
                    Index = state.CurrentIndex.ToString(),
                    HasMore = false
                };
            }

            // Compute next permutation in-place
            PermutationCalculator.NextPermutation(state.CurrentPermutation);
            state.CurrentIndex++;
        }

        await _stateStore.SaveAsync(sessionId, state);

        return new NextResponse
        {
            Permutation = state.CurrentPermutation.ToArray(),
            Index = state.CurrentIndex.ToString(),
            HasMore = state.CurrentIndex < state.TotalPermutations
        };
    }

    public async Task<PageResponse> GetPageAsync(Guid sessionId, int pageSize, int pageNumber)
    {
        var state = await _stateStore.GetAsync(sessionId)
            ?? throw new InvalidOperationException("Session not found. Call Start first.");

        if (pageSize < 1 || pageSize > 1000)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 1000.");

        BigInteger total = state.TotalPermutations;
        BigInteger startIndex = (BigInteger)(pageNumber - 1) * pageSize; // 0-based

        // Calculate total pages
        BigInteger totalPages = (total + pageSize - 1) / pageSize;

        if (startIndex >= total)
        {
            return new PageResponse
            {
                Permutations = new List<PermutationItem>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total.ToString("N0"),
                TotalPages = totalPages.ToString("N0"),
                HasNextPage = false,
                HasPreviousPage = pageNumber > 1
            };
        }

        // Calculate how many items on this page
        BigInteger remaining = total - startIndex;
        int count = (int)BigInteger.Min(remaining, pageSize);

        var permutations = new List<PermutationItem>(count);

        for (int i = 0; i < count; i++)
        {
            BigInteger idx = startIndex + i;
            var perm = PermutationCalculator.GetPermutationByIndex(state.N, idx);
            permutations.Add(new PermutationItem
            {
                Permutation = perm,
                Index = (idx + 1).ToString() // 1-based display index
            });
        }

        // Update state: current permutation = last one on this page
        var lastOnPage = permutations[^1];
        state.CurrentPermutation = lastOnPage.Permutation.ToArray();
        state.CurrentIndex = startIndex + count; // 1-based
        await _stateStore.SaveAsync(sessionId, state);

        return new PageResponse
        {
            Permutations = permutations,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total.ToString("N0"),
            TotalPages = totalPages.ToString("N0"),
            HasNextPage = startIndex + count < total,
            HasPreviousPage = pageNumber > 1
        };
    }

    public async Task ResetAsync(Guid sessionId)
    {
        await _stateStore.RemoveAsync(sessionId);
    }
}

using System.Numerics;
using PermutationGenerator.Calculators;
using PermutationGenerator.Exceptions;
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
        if (n < 1 || n > PermutationCalculator.MaxN)
            throw new ArgumentOutOfRangeException(nameof(n), $"n must be between 1 and {PermutationCalculator.MaxN}.");

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
            ?? throw new SessionNotFoundException(sessionId);

        await state.Semaphore.WaitAsync();
        try
        {
            if (state.CurrentPermutation.Length == 0)
            {
                state.CurrentPermutation = PermutationCalculator.GetFirstPermutation(state.N);
                state.CurrentIndex = 1;
            }
            else
            {
                if (state.CurrentIndex >= state.TotalPermutations)
                {
                    return new NextResponse
                    {
                        Permutation = Array.Empty<int>(),
                        Index = state.CurrentIndex.ToString(),
                        HasMore = false,
                        Message = "אין יותר קומבינציות."
                    };
                }

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
        finally
        {
            state.Semaphore.Release();
        }
    }

    public async Task<PageResponse> GetPageAsync(Guid sessionId, int pageSize, int pageNumber, long? fromIndex = null)
    {
        var state = await _stateStore.GetAsync(sessionId)
            ?? throw new SessionNotFoundException(sessionId);

        await state.Semaphore.WaitAsync();
        try
        {
            BigInteger total = state.TotalPermutations;

            // If fromIndex provided, use it as base offset for page 1
            BigInteger baseOffset = fromIndex.HasValue ? new BigInteger(fromIndex.Value) : 0;
            BigInteger startIndex = baseOffset + (BigInteger)(pageNumber - 1) * pageSize;
            BigInteger remainingTotal = total - baseOffset;
            BigInteger totalPages = (remainingTotal + pageSize - 1) / pageSize;

            if (startIndex >= total)
            {
                return new PageResponse
                {
                    Permutations = new List<PermutationItem>(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = remainingTotal.ToString("N0"),
                    TotalPages = totalPages.ToString("N0"),
                    HasNextPage = false,
                    HasPreviousPage = pageNumber > 1
                };
            }

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
                    Index = (idx + 1).ToString()
                });
            }

            var lastOnPage = permutations[^1];
            state.CurrentPermutation = lastOnPage.Permutation.ToArray();
            state.CurrentIndex = startIndex + count;
            await _stateStore.SaveAsync(sessionId, state);

            return new PageResponse
            {
                Permutations = permutations,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = remainingTotal.ToString("N0"),
                TotalPages = totalPages.ToString("N0"),
                HasNextPage = startIndex + count < total,
                HasPreviousPage = pageNumber > 1
            };
        }
        finally
        {
            state.Semaphore.Release();
        }
    }

    public async Task<NextResponse> GetCurrentAsync(Guid sessionId)
    {
        var state = await _stateStore.GetAsync(sessionId)
            ?? throw new SessionNotFoundException(sessionId);

        if (state.CurrentPermutation.Length == 0)
            return new NextResponse { Permutation = Array.Empty<int>(), Index = "0", HasMore = true };

        return new NextResponse
        {
            Permutation = state.CurrentPermutation.ToArray(),
            Index = state.CurrentIndex.ToString(),
            HasMore = state.CurrentIndex < state.TotalPermutations
        };
    }

    public async Task<BigInteger> GetCurrentIndexAsync(Guid sessionId)
    {
        var state = await _stateStore.GetAsync(sessionId)
            ?? throw new SessionNotFoundException(sessionId);
        return state.CurrentIndex;
    }

    public async Task ResetAsync(Guid sessionId)
    {
        await _stateStore.RemoveAsync(sessionId);
    }
}

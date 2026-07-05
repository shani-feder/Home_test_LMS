using System.Collections.Concurrent;

namespace PermutationGenerator.State;

public class MemoryPermutationStateStore : IPermutationStateStore
{
    private readonly ConcurrentDictionary<Guid, GeneratorState> _store = new();

    public Task<GeneratorState?> GetAsync(Guid sessionId)
    {
        _store.TryGetValue(sessionId, out var state);
        return Task.FromResult(state);
    }

    public Task SaveAsync(Guid sessionId, GeneratorState state)
    {
        _store[sessionId] = state;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Guid sessionId)
    {
        _store.TryRemove(sessionId, out _);
        return Task.CompletedTask;
    }
}

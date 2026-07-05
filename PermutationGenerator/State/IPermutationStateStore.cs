namespace PermutationGenerator.State;

public interface IPermutationStateStore
{
    /// <summary>
    /// Returns the live state object for the given session.
    /// The returned reference points directly to the in-memory object.
    /// Treat as immutable - always call SaveAsync to persist changes.
    /// </summary>
    Task<GeneratorState?> GetAsync(Guid sessionId);

    /// <summary>
    /// Persists the state for the given session, overwriting any existing value.
    /// </summary>
    Task SaveAsync(Guid sessionId, GeneratorState state);

    /// <summary>
    /// Removes the session state entirely (e.g. on reset).
    /// </summary>
    Task RemoveAsync(Guid sessionId);
}

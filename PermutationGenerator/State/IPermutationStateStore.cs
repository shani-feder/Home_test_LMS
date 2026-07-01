namespace PermutationGenerator.State;

public interface IPermutationStateStore
{
    Task<GeneratorState?> GetAsync(Guid sessionId);
    Task SaveAsync(Guid sessionId, GeneratorState state);
    Task RemoveAsync(Guid sessionId);
}

namespace PermutationGenerator.Exceptions;

public class SessionNotFoundException : Exception
{
    public SessionNotFoundException(Guid sessionId)
        : base($"Session '{sessionId}' not found. Call Start first.")
    {
    }
}

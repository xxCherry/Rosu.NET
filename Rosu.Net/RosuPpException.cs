namespace Rosu.Net;

public sealed class RosuPpException(RosuPpError error, string? message = null) : 
    Exception(message ?? $"{error}: {Native.ErrorString(error)}")
{
    public RosuPpError Error { get; } = error;
}

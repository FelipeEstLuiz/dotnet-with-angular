namespace Application.Domain.Util;

public static class ProtocolGenerator
{
    public static string SetProtocol() => $"{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid().ToString("N")[..8]}";
}
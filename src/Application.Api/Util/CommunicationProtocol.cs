namespace Application.Api.Util;

public class CommunicationProtocol
{
    public string Protocol { get; private set; } = null!;

    public void SetProtocol(string protocol) => Protocol = protocol;

    public override string ToString() => Protocol;
}

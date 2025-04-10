namespace Application.Api.Controllers._Shared;

public class CommunicationProtocol
{
    public string Protocol { get; private set; } = null!;

    public void SetProtocol(string protocol) => Protocol = protocol;

    public override string ToString() => Protocol;
}

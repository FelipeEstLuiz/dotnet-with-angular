using Application.Domain.Util;
using System.Text.RegularExpressions;

namespace Tests.Systems.Domain;

public class ProtocolGeneratorTests
{
    [Fact]
    public void SetProtocol_DeveGerarProtocoloComFormatoEsperado()
    {
        string protocol = ProtocolGenerator.SetProtocol();

        Assert.False(string.IsNullOrWhiteSpace(protocol));
        Assert.Equal(22, protocol.Length);

        Regex regex = new(@"^\d{14}[a-fA-F0-9]{8}$");
        Assert.Matches(regex, protocol);
    }

    [Fact]
    public void SetProtocol_DeveGerarProtocoloUnico()
    {
        string p1 = ProtocolGenerator.SetProtocol();
        string p2 = ProtocolGenerator.SetProtocol();

        Assert.NotEqual(p1, p2);
    }
}

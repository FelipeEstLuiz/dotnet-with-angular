using Application.Domain.Util;
using System.Text.RegularExpressions;

namespace Tests.Domain;

public class ProtocolGeneratorTests
{
    [Fact]
    public void SetProtocol_DeveGerarProtocoloComFormatoEsperado()
    {
        string protocolo = ProtocolGenerator.SetProtocol();

        Assert.False(string.IsNullOrWhiteSpace(protocolo));
        Assert.Equal(22, protocolo.Length);

        Regex regex = new(@"^\d{14}[a-fA-F0-9]{8}$");
        Assert.Matches(regex, protocolo);
    }

    [Fact]
    public void SetProtocol_DeveGerarProtocoloUnico()
    {
        string p1 = ProtocolGenerator.SetProtocol();
        string p2 = ProtocolGenerator.SetProtocol();

        Assert.NotEqual(p1, p2);
    }
}

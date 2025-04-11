using Application.Domain.Exception;
using Application.Domain.Util;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Tests.Domain;

public  class ClsGlobalTests
{
    [Fact]
    public void GetTokenKey_DeveRetornarByteArray_QuandoChaveForValida()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:SecretKey", new string('x', 64) }
            })
            .Build();

        byte[] result = ClsGlobal.GetTokenKey(config);

        Assert.NotNull(result);
        Assert.Equal(64, result.Length);
        Assert.Equal(Encoding.ASCII.GetBytes(new string('x', 64)), result);
    }

    [Fact]
    public void GetTokenKey_DeveLancarException_QuandoChaveNaoForEncontrada()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddInMemoryCollection([])
            .Build();

        ValidationException ex = Assert.Throws<ValidationException>(() => ClsGlobal.GetTokenKey(config));
        Assert.Equal("Token nao encontrado no arquivo appsettings", ex.Message);
    }

    [Fact]
    public void GetTokenKey_DeveLancarException_QuandoChaveForMuitoCurta()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:SecretKey", "123456" }
            })
            .Build();

        ValidationException ex = Assert.Throws<ValidationException>(() => ClsGlobal.GetTokenKey(config));
        Assert.Equal("Token de autenticacao invalido.O tamanho minimo e 64 caracteres.", ex.Message);
    }
}

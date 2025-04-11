using Application.Domain.Exception;
using System.Net;

namespace Tests.Domain;

public class ValidationExtensionsTests
{
    [Fact]
    public void ThrowIfNull_DeveLancarExcecao_QuandoObjetoForNulo()
    {
        object? obj = null;

#pragma warning disable CS8634
        ValidationException exception = Assert.Throws<ValidationException>(() => obj.ThrowIfNull(nameof(obj)));
#pragma warning restore CS8634

        Assert.Equal($"{nameof(obj)} nao pode ser nulo.", exception.Message);
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
    }

    [Fact]
    public void ThrowIfNull_DeveNaoLancarExcecao_QuandoObjetoNaoForNulo()
    {
        object obj = new();
        obj.ThrowIfNull(nameof(obj));
    }

    [Fact]
    public void ThrowIfNullOrEmptyString_DeveLancarExcecao_QuandoStringForNulaOuVazia()
    {
        string? str = " ";
        ValidationException exception = Assert.Throws<ValidationException>(() => str.ThrowIfNullOrEmpty(nameof(str)));
        Assert.Equal($"{nameof(str)} nao pode ser nulo ou vazio.", exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrEmptyString_DeveNaoLancarExcecao_QuandoStringForValida()
    {
        string str = "valido";
        str.ThrowIfNullOrEmpty(nameof(str));
    }

    [Fact]
    public void ThrowIfNullOrEmptyCollection_DeveLancarExcecao_QuandoListaForVazia()
    {
        List<string> lista = [];

        ValidationException exception = Assert.Throws<ValidationException>(() => lista.ThrowIfNullOrEmpty(nameof(lista)));
        Assert.Equal($"{nameof(lista)} nao pode ser nulo ou vazio.", exception.Message);
    }

    [Fact]
    public void ThrowIfNullOrEmptyCollection_NaoDeveLancarExcecao_QuandoListaNaoForVazia()
    {
        List<string> lista = ["nao lancar"];

        lista.ThrowIfNullOrEmpty(nameof(lista));
    }

    [Fact]
    public void ThrowIfNullOrEmptyCollection_DeveLancarExcecao_QuandoListaForNull()
    {
        List<string>? lista = null;

#pragma warning disable CS8604
        ValidationException exception = Assert.Throws<ValidationException>(() => lista.ThrowIfNullOrEmpty(nameof(lista)));
#pragma warning restore CS8604
        Assert.Equal($"{nameof(lista)} nao pode ser nulo ou vazio.", exception.Message);
    }

    [Fact]
    public void ThrowIfFalse_DeveLancarExcecao_QuandoCondicaoForFalse()
    {
        ValidationException exception = Assert.Throws<ValidationException>(() => false.ThrowIfFalse("Falhou"));
        Assert.Equal("Falhou", exception.Message);
    }

    [Fact]
    public void ThrowIfFalse_DeveNaoLancarExcecao_QuandoCondicaoForTrue()
    {
        bool verdadeiro = true;
        verdadeiro.ThrowIfFalse("Não deve lançar");
    }

    [Fact]
    public void ThrowIfFalse_DeveNaoLancarExcecao_QuandoWhenForFalse() => ValidationException.When(false, "Não deve lançar");

    [Fact]
    public void ThrowIfTrue_DeveLancarExcecao_QuandoWhenForTrue()
    {
        ValidationException exception = Assert.Throws<ValidationException>(() => ValidationException.When(true, "Falhou"));
        Assert.Equal("Falhou", exception.Message);
    }

    [Theory(DisplayName = "ThrowIfTrue_DeveLancarExcecao_QuandoWhenForTrue_StatusCode")]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    public void ThrowIfTrue_DeveLancarExcecao_QuandoWhenForTrue_StatusCode(HttpStatusCode httpStatusCode)
    {
        ValidationException exception = Assert.Throws<ValidationException>(() => ValidationException.When(
            true,
            "Falhou",
            httpStatusCode: httpStatusCode
        ));
        Assert.Equal("Falhou", exception.Message);
        Assert.Equal(httpStatusCode, exception.StatusCode);
    }
}

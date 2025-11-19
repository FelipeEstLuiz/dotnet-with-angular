using Application.Domain.Enums;
using Application.Domain.Model;
using Bogus;

namespace Tests.Systems.Domain;

public class ResultTests
{
    [Fact(DisplayName = "Result com - IsSuccess = true, IsFailure = false, Errors array vazio e ResponseCode = default (None)")]
    public void Result_Return_IsSuccess()
    {
        Result<bool> result = Result<bool>.Success(true);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.False(result.Errors.Any());
        Assert.Equal(ResponseCodes.NONE, result.ResponseCode);

        Assert.Equal(0, result.CurrentPage);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(0, result.TotalItems);
    }

    [Fact(DisplayName = "Result com - IsSuccess = false, IsFailure = true, Errors array com valor, Errors com o erro informado e ResponseCode = default (None)")]
    public void Result_Return_IsFailure()
    {
        string mensagemErro = "Erro";
        Result<bool> result = Result<bool>.Failure(mensagemErro);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.True(result.Errors.Any());
        Assert.Contains(mensagemErro, result.Errors);
        Assert.Equal(ResponseCodes.NONE, result.ResponseCode);

        Assert.Equal(0, result.CurrentPage);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(0, result.TotalItems);
    }

    [Fact(DisplayName = "Result com - IsFailure = true, Errors com multiplas mensagens")]
    public void Result_Return_IsFailure_MultipleErrors()
    {
        string mensagemErro1 = "Erro 1";
        string mensagemErro2 = "Erro 2";
        Result<bool> result = Result<bool>.Failure([mensagemErro1, mensagemErro2]);

        Assert.True(result.IsFailure);
        Assert.Contains(mensagemErro1, result.Errors);
        Assert.Contains(mensagemErro2, result.Errors);
    }

    [Theory(DisplayName = "Result com - ResponseCodes com valor informado")]
    [InlineData(ResponseCodes.BAD_REQUEST)]
    [InlineData(ResponseCodes.NONE)]
    public void Result_Return_IsFailure_ResponseCodes(ResponseCodes responseCodes)
    {
        Result<bool> result = Result<bool>.Failure("Erro", responseCodes);

        Assert.Equal(responseCodes, result.ResponseCode);
    }

    [Fact(DisplayName = "Result com - IsSuccess = true e metodo ToString() retornando mensagem de sucesso")]
    public void Result_Return_IsSuccess_ToString()
    {
        Result<bool> result = Result<bool>.Success(true);

        Assert.True(result.IsSuccess);
        Assert.Equal("true", result.ToString());
    }

    [Fact(DisplayName = "Result com - IsFailure = true e metodo ToString() retornando mensagem de erro")]
    public void Result_Return_IsFailure_ToString()
    {
        string mensagemErro = "Erro";

        Result<bool> result = Result<bool>.Failure(mensagemErro);

        Assert.True(result.IsFailure);
        Assert.Contains(mensagemErro, result.ToString());
    }

    [Fact(DisplayName = "Result com - IsFailure = true e metodo ToString() retornando mensagens dos erros")]
    public void Result_Return_IsFailure_MultipleErrors_ToString()
    {
        string mensagemErro1 = "Erro 1";
        string mensagemErro2 = "Erro 2";

        Result<bool> result = Result<bool>.Failure([mensagemErro1, mensagemErro2]);

        Assert.True(result.IsFailure);
        Assert.Contains(mensagemErro1, result.ToString());
        Assert.Contains(mensagemErro2, result.ToString());
        Assert.Contains(";", result.ToString());
    }

    [Theory(DisplayName = "Result com - IsSuccess = true, Data = boolean")]
    [InlineData(true)]
    [InlineData(false)]
    public void Result_Return_IsSuccess_Data(bool dataValue)
    {
        Result<bool> result = Result<bool>.Success(dataValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(dataValue, result.Data);
    }

    [Theory(DisplayName = "Result com - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void Result_Return_IsSuccess_Data_Object(Cliente cliente)
    {
        Result<Cliente> result = Result<Cliente>.Success(cliente);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Nome);
        Assert.True(result.Data.Idade >= 0);
    }

    [Theory(DisplayName = "Result com implicit operator - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void Result_Return_IsSuccess_Data_Object_Implicit_Operator(Cliente cliente)
    {
        Result<Cliente> result = cliente;

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Nome);
        Assert.True(result.Data.Idade >= 0);
    }

    [Theory(DisplayName = "Result Server Side com - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void ResultServerSide_Return_IsSuccess_Data_Object(Cliente cliente)
    {
        Result<Cliente> result = Result<Cliente>.Success(cliente, 1, 1, 1, 1);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        Assert.Equal(1, result.TotalItems);
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(1, result.PageSize);
    }

    [Fact]
    public void AddErrorIf_QuandoCondicaoForVerdadeira_DeveAdicionarErro()
    {
        // Dado
        // Quando
        Result<string> result = Result<string>.Success("ok").AddErrorIf(true, "falhou");

        // Então
        Assert.True(result.IsFailure);
        Assert.Contains("falhou", result.Errors);
    }

    [Fact]
    public void AddErrorIf_QuandoACordicaoForFalse_NaoDeveAdicionarErro()
    {
        // Dado
        // Quando
        Result<string> result = Result<string>.Success("ok").AddErrorIf(false, "falhou");

        // Então
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Map_QuandoSucesso_DeveTransformarDados()
    {
        // Dado
        // Quando
        Result<int> result = Result<int>.Success(5)
            .Map(x => x * 2);

        // Então
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Data);
    }

    [Fact]
    public void Map_QuandoFalha_DevePropagarErros()
    {
        // Dado
        // Quando
        Result<int> result = Result<int>.Failure("erro").Map(x => x * 2);

        // Então
        Assert.True(result.IsFailure);
        Assert.Contains("erro", result.Errors);
    }

    [Fact]
    public void Map_QuandoExcecao_DeveRetornarFalhaComMensagem()
    {
        // Dado
        // Quando
        Result<int> result = Result<int>.Success(5)
            .Map<int>(_ => throw new InvalidOperationException("falha no map"));

        // Então
        Assert.True(result.IsFailure);
        Assert.Contains("falha no map", result.Errors.First());
    }

    [Fact]
    public void Bind_QuandoSucesso_DeveEncadearResultado()
    {
        // Dado
        // Quando
        Result<string> result = Result<int>.Success(5)
            .Bind(x => Result<string>.Success($"Value: {x}"));

        // Então
        Assert.True(result.IsSuccess);
        Assert.Equal("Value: 5", result.Data);
    }

    [Fact]
    public void Bind_QuandoFalha_DevePropagarErros()
    {
        // Dado
        // Quando
        Result<string> result = Result<int>.Failure("falhou")
            .Bind(x => Result<string>.Success($"Value: {x}"));

        // Então
        Assert.True(result.IsFailure);
        Assert.Contains("falhou", result.Errors);
    }

    [Fact]
    public void Try_QuandoSemExcecao_DeveRetornarSucesso()
    {
        // Dado
        // Quando
        Result<int> result = Result<int>.Try(() => 42);

        // Então
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Data);
    }

    [Fact]
    public void Try_QuandoExcecao_DeveRetornarFalha()
    {
        // Dado
        // Quando
        Result<int> result = Result<int>.Try(() => throw new Exception("falhou"));

        // Então
        Assert.True(result.IsFailure);
        Assert.Contains("falhou", result.Errors);
    }

    [Fact]
    public void Data_CapturarDataQuandoFalha_DeveRetornarValorDefault()
    {
        // Dado

        // Quando
        Result<decimal> result = Result.Failure<decimal>("falhou");

        // Então
        Assert.Equal(default, result.Data);
    }

    [Fact]
    public void Match_QuandoSucesso_DeveExecutarOnSuccess()
    {
        // Dado
        string? captured = null;

        // Quando
        Result<string> result = Result<string>.Success("ok");

        result.Match(
            onSuccess: x => captured = x,
            onFailure: _ => captured = "fail"
        );

        // Então
        Assert.Equal("ok", captured);
    }

    [Fact]
    public void Match_QuandoSucesso_DeveRetornarNovoResult()
    {
        // Dado
        string? captured = null;

        // Quando
        Result<string> result = Result<string>.Success("ok");

        Result<bool> newResult = result.Match(
            onSuccess: x =>
            {
                captured = x;
                return Result<bool>.Success(true);
            },
            onFailure: errors =>
            {
                captured = "fail";
                return Result<bool>.Failure(errors);
            }
        );

        // Então
        Assert.Equal("ok", captured);
        Assert.True(newResult.IsSuccess);
    }

    [Fact]
    public void Match_QuandoFalha_DeveExecutarOnFailure()
    {
        // Dado
        string? captured = null;

        // Quando
        Result<string> result = Result<string>.Failure("erro");

        result.Match(
            onSuccess: _ => captured = "ok",
            onFailure: e => captured = e.First()
        );

        // Então
        Assert.Equal("erro", captured);
    }

    [Fact]
    public void Match_QuandoFalha_DeveRetornarNovoResult()
    {
        // Dado
        string? captured = null;

        // Quando
        Result<string> result = Result<string>.Failure("erro");

        Result<bool> newResult = result.Match(
            onSuccess: _ =>
            {
                captured = "ok";
                return Result<bool>.Success(true);
            },
            onFailure: e =>
            {
                captured = e.First();
                return Result<bool>.Failure(e);
            }
        );

        // Então
        Assert.Equal("erro", captured);
        Assert.True(newResult.IsFailure);
        Assert.Contains("erro", newResult.Errors);
    }

    public static IEnumerable<object[]> ObterClientes()
    {
        Faker<Cliente> faker = new Faker<Cliente>().CustomInstantiator(f => new Cliente(
            f.Name.FullName(),
            f.Random.Int(18, 60)
        ));

        yield return new object[] { faker.Generate() };
        yield return new object[] { faker.Generate() };
        yield return new object[] { faker.Generate() };
    }

    public record Cliente(string Nome, int Idade)
    {
        public string Nome { get; set; } = Nome;
        public int Idade { get; set; } = Idade;
    }

    public record Cliente2(string Nome, int Idade)
    {
        public string Nome { get; set; } = Nome;
        public int Idade { get; set; } = Idade;
    }
}

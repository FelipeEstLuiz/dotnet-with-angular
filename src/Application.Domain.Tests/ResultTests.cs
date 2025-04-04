using Application.Domain.Enums;
using Application.Domain.Model;
using Bogus;
using FluentAssertions;

namespace Application.Domain.Tests;

public class ResultTests
{
    [Fact(DisplayName = "Result com - IsSuccess = true, IsFailure = false, Errors array vazio e ResponseCode = default (None)")]
    public void Result_Return_IsSuccess()
    {
        Result<bool> result = Result<bool>.Success(true);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.ResponseCode.Should().Be(ResponseCodes.NONE);
    }

    [Fact(DisplayName = "Result com - IsSuccess = false, IsFailure = true, Errors array com valor, Errors com o erro informado e ResponseCode = default (None)")]
    public void Result_Return_IsFailure()
    {
        string mensagemErro = "Erro";
        Result<bool> result = Result<bool>.Failure(mensagemErro);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(mensagemErro);
        result.ResponseCode.Should().Be(ResponseCodes.NONE);
    }

    [Fact(DisplayName = "Result com - IsFailure = true, Errors com multiplas mensagens")]
    public void Result_Return_IsFailure_MultipleErrors()
    {
        string mensagemErro1 = "Erro 1";
        string mensagemErro2 = "Erro 2";
        Result<bool> result = Result<bool>.Failure([mensagemErro1, mensagemErro2]);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(mensagemErro1);
        result.Errors.Should().Contain(mensagemErro2);
    }

    [Theory(DisplayName = "Result com - ResponseCodes com valor informado")]
    [InlineData(ResponseCodes.BAD_REQUEST)]
    [InlineData(ResponseCodes.NONE)]
    public void Result_Return_IsFailure_ResponseCodes(ResponseCodes responseCodes)
    {
        Result<bool> result = Result<bool>.Failure("Erro", responseCodes);

        result.ResponseCode.Should().Be(responseCodes);
    }

    [Fact(DisplayName = "Result com - IsSuccess = true e metodo ToString() retornando mensagem de sucesso")]
    public void Result_Return_IsSuccess_ToString()
    {
        Result<bool> result = Result<bool>.Success(true);

        result.IsSuccess.Should().BeTrue();
        result.ToString().Should().Be("Success");
    }

    [Fact(DisplayName = "Result com - IsFailure = true e metodo ToString() retornando mensagem de erro")]
    public void Result_Return_IsFailure_ToString()
    {
        string mensagemErro = "Erro";

        Result<bool> result = Result<bool>.Failure(mensagemErro);

        result.IsFailure.Should().BeTrue();
        result.ToString().Should().Contain(mensagemErro);
    }

    [Fact(DisplayName = "Result com - IsFailure = true e metodo ToString() retornando mensagens dos erros")]
    public void Result_Return_IsFailure_MultipleErrors_ToString()
    {
        string mensagemErro1 = "Erro 1";
        string mensagemErro2 = "Erro 2";

        Result<bool> result = Result<bool>.Failure([mensagemErro1, mensagemErro2]);

        result.IsFailure.Should().BeTrue();
        result.ToString().Should().Contain(mensagemErro1);
        result.ToString().Should().Contain(mensagemErro2);
        result.ToString().Should().Contain(";");
    }

    [Theory(DisplayName = "Result com - IsSuccess = true, Data = boolean")]
    [InlineData(true)]
    [InlineData(false)]
    public void Result_Return_IsSuccess_Data(bool dataValue)
    {
        Result<bool> result = Result<bool>.Success(dataValue);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(dataValue);
    }

    [Theory(DisplayName = "Result com - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void Result_Return_IsSuccess_Data_Object(Cliente cliente)
    {
        Result<Cliente> result = Result<Cliente>.Success(cliente);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Nome.Should().NotBeNull();
        result.Data.Idade.Should().BeGreaterThanOrEqualTo(0);
    }

    [Theory(DisplayName = "Result com implicit operator - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void Result_Return_IsSuccess_Data_Object_Implicit_Operator(Cliente cliente)
    {
        Result<Cliente> result = cliente;

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Nome.Should().NotBeNull();
        result.Data.Idade.Should().BeGreaterThanOrEqualTo(0);
    }

    [Theory(DisplayName = "Result SetResult com - IsSuccess = true, Data = object")]
    [MemberData(nameof(ObterClientes))]
    public void Result_SetResult_Return_IsSuccess_Data_Object(Cliente cliente)
    {
        Result<Cliente> resultCliente = cliente;

        Result<Cliente2> result = resultCliente
            .SetResult(cliente => new Cliente2(cliente.Nome, cliente.Idade));

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Nome.Should().NotBeNull();
        result.Data.Idade.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact(DisplayName = "Result SetResult com - IsFailure = true, Data = object e ResponseCodes = default")]
    public void Result_SetResult_Return_IsFailure_Data()
    {
        string mensagemErro = "Erro";
        Result<Cliente> resultCliente = Result<Cliente>.Failure(mensagemErro);

        Result<Cliente2> result = resultCliente
           .SetResult(cliente => new Cliente2(cliente.Nome, cliente.Idade));

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(mensagemErro);
        result.ResponseCode.Should().Be(ResponseCodes.NONE);
    }

    [Fact(DisplayName = "Result SetResult com - IsFailure = true")]
    public void Result_SetResult_Return_IsFailure()
    {
        string mensagemErro = "Erro";
        Result<Cliente> resultCliente = Result<Cliente>.Failure(mensagemErro);

        Result<Cliente2> result = resultCliente.SetResult<Cliente2>();

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(mensagemErro);
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
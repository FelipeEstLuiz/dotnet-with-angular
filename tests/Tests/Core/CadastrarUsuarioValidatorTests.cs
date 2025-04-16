using Application.Core.Model;
using Application.Core.Validator;
using Bogus;

namespace Tests.Core;

public class CadastrarUsuarioValidatorTests
{
    private readonly CadastrarUsuarioModel _command;

    public CadastrarUsuarioValidatorTests()
    {
        Faker<CadastrarUsuarioModel> faker = new Faker<CadastrarUsuarioModel>()
            .RuleFor(cmd => cmd.Nome, f => f.Name.FullName())
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(cmd => cmd.Senha, f => f.Internet.Password(8))
            .RuleFor(cmd => cmd.SenhaConfirmacao, (f, cmd) => cmd.Senha);

        _command = faker.Generate();
    }

    [Fact]
    public void Deve_Retornar_Erros_Se_Command_For_Invalido()
    {
        CadastrarUsuarioValidator validator = new();
        CadastrarUsuarioModel command = new()
        {
            Nome = "",
            Email = "invalido",
            Senha = "abc",
            SenhaConfirmacao = "diferente"
        };

        FluentValidation.Results.ValidationResult result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage.Contains("Obrigatorio"));
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("Invalido"));
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha" && e.ErrorMessage.Contains("Deve ter pelo menos 8 caracteres."));
        Assert.Contains(result.Errors, e => e.PropertyName == "SenhaConfirmacao" && e.ErrorMessage.Contains("nao corresponde"));
    }


    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_Email_Invalido")]
    [InlineData("")]
    [InlineData("teste.teste")]
    [InlineData("teste@teste.comeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Email_Invalido(string email)
    {
        _command.Email = email;

        CadastrarUsuarioValidator validator = new();

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_Nome_Invalido")]
    [InlineData("")]
    [InlineData("te")]
    [InlineData("comeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Nome_Invalido(string nome)
    {
        CadastrarUsuarioValidator validator = new();

        _command.Nome = nome;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Nome");
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Vazia()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = string.Empty;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Obrigatorio"));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Menor8Caracteres()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = "t87@De";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Deve ter pelo menos 8 caracteres."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Deve_Conter_Numeros()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = "t@Dertewq";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Deve conter pelo menos um numero."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Deve_Conter_Letras_Maiusculas()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = "t87@rrw422e";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Deve conter pelo menos uma letra maiuscula."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Deve_Conter_Letras_Minusculas()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = "87@D432524562456";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Deve conter pelo menos uma letra minuscula."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Senha_Deve_Conter_Caractres_Especiais()
    {
        CadastrarUsuarioValidator validator = new();

        _command.Senha = "87tD432524562456";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Deve conter pelo menos um caractere especial (@#$%^&+=!)."));
    }

    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_SenhaConfirmacao_Invalido")]
    [InlineData("")]
    [InlineData("asd#4rRrrrrrr")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_SenhaConfirmacao_Invalido(string senhaConfirmacao)
    {
        CadastrarUsuarioValidator validator = new();

        _command.SenhaConfirmacao = senhaConfirmacao;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "SenhaConfirmacao");
    }
}

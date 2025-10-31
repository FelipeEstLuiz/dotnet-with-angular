using Application.Core.Model;
using Application.Core.Validator;
using Bogus;

namespace Tests.Systems.Core;

public class CadastrarUsuarioValidatorTests
{
    private readonly InsertUserModel _command;

    public CadastrarUsuarioValidatorTests()
    {
        Faker<InsertUserModel> faker = new Faker<InsertUserModel>()
            .RuleFor(cmd => cmd.Name, f => f.Name.FullName())
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(cmd => cmd.Password, f => f.Internet.Password(8))
            .RuleFor(cmd => cmd.PasswordConfirmed, (f, cmd) => cmd.Password)
            .RuleFor(u => u.DateOfBirth, f =>
            {
                DateTime date = f.Date.Past(50, DateTime.Today.AddYears(-18));
                return DateOnly.FromDateTime(date);
            })
            .RuleFor(u => u.Introduction, f => f.Lorem.Sentence())
            .RuleFor(u => u.Gender, f => f.PickRandom("Masculino", "Feminino", "Outro"))
            .RuleFor(u => u.KnowAs, f => f.Name.FirstName());

        _command = faker.Generate();
    }

    [Fact]
    public void Deve_Retornar_Erros_Se_Command_For_Invalido()
    {
        InsertUserValidator validator = new();
        InsertUserModel command = new()
        {
            Name = "",
            Email = "invalido",
            Password = "abc",
            PasswordConfirmed = "diferente"
        };

        FluentValidation.Results.ValidationResult result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("Required"));
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage.Contains("Invalid"));
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage.Contains("It must have at least 8 characters."));
        Assert.Contains(result.Errors, e => e.PropertyName == "PasswordConfirmed" && e.ErrorMessage.Contains("The password confirmation does not match the password"));
    }


    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_Email_Invalido")]
    [InlineData("")]
    [InlineData("teste.teste")]
    [InlineData("teste@teste.comeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Email_Invalido(string email)
    {
        _command.Email = email;

        InsertUserValidator validator = new();

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_Nome_Invalido")]
    [InlineData("")]
    [InlineData("te")]
    [InlineData("comeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Name_Invalido(string nome)
    {
        InsertUserValidator validator = new();

        _command.Name = nome;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Vazia()
    {
        InsertUserValidator validator = new();

        _command.Password = string.Empty;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Required"));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Menor8Caracteres()
    {
        InsertUserValidator validator = new();

        _command.Password = "t87@De";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("It must have at least 8 characters."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Deve_Conter_Numeros()
    {
        InsertUserValidator validator = new();

        _command.Password = "t@Dertewq";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("It must have at least one number."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Deve_Conter_Letras_Maiusculas()
    {
        InsertUserValidator validator = new();

        _command.Password = "t87@rrw422e";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("It must have at least one capital letter."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Deve_Conter_Letras_Minusculas()
    {
        InsertUserValidator validator = new();

        _command.Password = "87@D432524562456";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("It must have at least one lowercase letter."));
    }

    [Fact]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_Password_Deve_Conter_Caractres_Especiais()
    {
        InsertUserValidator validator = new();

        _command.Password = "87tD432524562456";

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("It must have at least one special character (@#$%^&+=!)."));
    }

    [Theory(DisplayName = "Validator_Deve_Retornar_Erro_Se_PasswordConfirmed_Invalido")]
    [InlineData("")]
    [InlineData("asd#4rRrrrrrr")]
    public void CadastrarUsuarioValidator_Deve_Retornar_Erro_Se_PasswordConfirmed_Invalido(string passwordConfirmed)
    {
        InsertUserValidator validator = new();

        _command.PasswordConfirmed = passwordConfirmed;

        FluentValidation.Results.ValidationResult result = validator.Validate(_command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PasswordConfirmed");
    }
}

using Application.Domain.Exception;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Application.Domain.Util;

public static class ClsGlobal
{
    public static byte[] GetTokenKey(IConfiguration configuration)
    {
        string secretKey = configuration["Jwt:SecretKey"]
            ?? throw new ValidationException("Token nao encontrado no arquivo appsettings");

        ValidationException.When(
            secretKey.Length < 64,
            "Token de autenticacao invalido.O tamanho minimo e 64 caracteres."
        );

        return Encoding.ASCII.GetBytes(secretKey);
    }
}

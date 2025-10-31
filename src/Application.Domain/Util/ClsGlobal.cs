using Application.Domain.Exception;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Application.Domain.Util;

public static class ClsGlobal
{
    public static byte[] GetTokenKey(IConfiguration configuration)
    {
        string secretKey = configuration["Jwt:SecretKey"]
            ?? throw new ValidationException("Token not found on appsettings");

        ValidationException.When(
            secretKey.Length < 64,
            "Invalid authentication token. The minimum size is 64 characters."
        );

        return Encoding.ASCII.GetBytes(secretKey);
    }
}

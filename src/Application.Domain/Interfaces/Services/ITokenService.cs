using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GerarToken(Usuario usuario);
}

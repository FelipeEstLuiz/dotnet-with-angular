using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Services;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}

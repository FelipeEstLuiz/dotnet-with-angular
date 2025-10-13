using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdateUserUseCase(
    IUserRepository usuarioRepository
) : IRequestHandler<UpdateUserModel, Result<string>>
{
    public async Task<Result<string>> Handle(
        UpdateUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<Domain.Entities.User?> resultUsuario = await usuarioRepository.GetByIdAsync(
            request.Id,
            cancellationToken
        );

        if (resultUsuario.IsSuccess && resultUsuario.Data is null)
            return Result<string>.Failure("Usuario nao encontrado");
        else if (resultUsuario.IsFailure)
            return resultUsuario.SetResult<string>();

        Domain.Entities.User usuario = resultUsuario.Data!;

        if (!string.Equals(usuario.UserName, request.NameToken, StringComparison.InvariantCulture))
            return Result<string>.Failure("Usuario invalido");

        usuario.SetCountry(request.Country);
        usuario.SetCity(request.City);
        usuario.SetIntroduction(request.Introduction);
        usuario.SetInterests(request.Interests);
        usuario.SetLookingFor(request.LookingFor);

        Result<bool> resultUpdate = await usuarioRepository.UpdateAsync(usuario, cancellationToken);

        return resultUpdate.SetResult(res => "Usuario atualizado com sucesso");
    }
}

using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.User;

public class UpdateUserUseCase(
    IUserRepository usuarioRepository
) : IRequestHandler<UpdateUserModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<Domain.Entities.User?> resultUsuario = await usuarioRepository.GetByIdAsync(
            request.Id,
            cancellationToken
        );

        if (resultUsuario.IsSuccess && resultUsuario.Data is null)
            return Result<bool>.Failure("Usuario nao encontrado");
        else if (resultUsuario.IsFailure)
            return resultUsuario.SetResult<bool>();

        Domain.Entities.User usuario = resultUsuario.Data!;

        usuario.SetName(request.Name);
        usuario.SetCountry(request.Country);
        usuario.SetCity(request.City);
        usuario.SetIntroduction(request.Introduction);
        usuario.SetInterests(request.Interests);
        usuario.SetLookingFor(request.LookingFor);

        return await usuarioRepository.UpdateAsync(usuario, cancellationToken);
    }
}

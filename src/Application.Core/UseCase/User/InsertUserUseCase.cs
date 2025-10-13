using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.User;

public class InsertUserUseCase(
    IUserRepository usuarioRepository,
    ITokenService tokenService
) : IRequestHandler<InsertUserModel, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(
        InsertUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Result<Domain.Entities.User?> resultUsuario = await usuarioRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        if (resultUsuario.IsSuccess && resultUsuario.Data is not null)
            return Result<LoginDto>.Failure("E-mail ja cadastrado");
        else if (resultUsuario.IsFailure)
            return Result<LoginDto>.Failure(resultUsuario.Errors);

        Domain.Entities.User usuario = request.MapUsuario();

        PasswordHasher<Domain.Entities.User> hasher = new();

        usuario.SetPassword(hasher.HashPassword(usuario, request.Password));

        Result<bool> resultInsert = await usuarioRepository.InsertAsync(usuario, cancellationToken);

        return resultInsert.IsSuccess
            ? Result<LoginDto>.Success(new LoginDto(usuario.Id, usuario.UserName, usuario.Email, await tokenService.GerarToken(usuario)))
            : Result<LoginDto>.Failure(resultInsert.Errors);
    }
}

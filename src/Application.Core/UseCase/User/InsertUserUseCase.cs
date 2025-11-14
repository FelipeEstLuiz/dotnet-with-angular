using Application.Core.DTO.User;
using Application.Core.Model.User;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.User;

public class InsertUserUseCase(
    IUserRepository userRepository,
    ITokenService tokenService
) : IRequestHandler<InsertUserModel, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(
        InsertUserModel request,
        CancellationToken cancellationToken = default
    )
    {
        Domain.Entities.User? resultUser = await userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken
        );

        if (resultUser is not null)
            return Result<LoginDto>.Failure("E-mail already exists");

        Domain.Entities.User user = request.MapUsuario();

        PasswordHasher<Domain.Entities.User> hasher = new();

        user.SetPassword(hasher.HashPassword(user, request.Password));

        await userRepository.AddAsync(user, cancellationToken);

        return await userRepository.SaveChangesAsync(cancellationToken)
            ? Result<LoginDto>.Success(new LoginDto(user.Id, user.UserName, user.Email, await tokenService.GerarToken(user), user.ImageUrl))
            : Result<LoginDto>.Failure("Error to adding new user");
    }
}

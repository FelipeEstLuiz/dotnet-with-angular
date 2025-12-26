using Application.Core.Model;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.UseCase.Logout;

public class LogoutUseCase(UserManager<Domain.Entities.User> userManager) : IRequestHandler<LogoutModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutModel request, CancellationToken cancellationToken = default)
    {
        await userManager
            .Users
            .Where(x => x.Id == request.UserId)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.RefreshToken, _ => null)
                    .SetProperty(x => x.RefreshTokenExpiry, _ => DateTime.MinValue),
                cancellationToken: cancellationToken
            );

        return Result.IsSuccess();
    }
}

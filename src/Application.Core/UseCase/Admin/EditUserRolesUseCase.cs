using Application.Core.Model.Admin;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.UseCase.Admin;

public class EditUserRolesUseCase(UserManager<Domain.Entities.User> userManager) : IRequestHandler<EditUserRolesModel, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(EditUserRolesModel request, CancellationToken cancellationToken = default)
    {
        string[] roles = request.Roles.Split(",");

        Domain.Entities.User? user = await userManager.FindByIdAsync(request.UserId);

        if (user is null) return Result<IEnumerable<string>>.Failure("Could not retrieve user");

        IList<string> userRoles = await userManager.GetRolesAsync(user);

        IdentityResult result = await userManager.AddToRolesAsync(user, roles.Except(userRoles));

        if (!result.Succeeded) return Result<IEnumerable<string>>.Failure("Failed to add to roles");

        result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(roles));

        if (!result.Succeeded) return Result<IEnumerable<string>>.Failure("Failed to remove from roles");

        IList<string> userRolesResult = await userManager.GetRolesAsync(user);

        return Result.Success<IEnumerable<string>>(userRolesResult);
    }
}

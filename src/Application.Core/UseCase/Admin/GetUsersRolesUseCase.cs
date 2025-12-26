using Application.Core.DTO.Admin;
using Application.Core.Model.Admin;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.UseCase.Admin;

public class GetUsersRolesUseCase(UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<GetUsersRolesModel, Result<IEnumerable<UsersRolesDto>>>
{
    public async Task<Result<IEnumerable<UsersRolesDto>>> Handle(GetUsersRolesModel request, CancellationToken cancellationToken = default)
    {
        List<Domain.Entities.User> users = await userManager.Users.ToListAsync(cancellationToken);
        List<UsersRolesDto> result = [];

        foreach (Domain.Entities.User user in users)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            result.Add(new(user.Id, user.Email!, roles));
        }

        return result;
    }
}

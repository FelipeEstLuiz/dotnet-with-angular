using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class GetGroupUseCase(IUnitOfWork unitOfWork) : IRequestHandler<GetGroupModel, Result<GroupDto?>>
{
    public async Task<Result<GroupDto?>> Handle(GetGroupModel request, CancellationToken cancellationToken = default)
    {
        Group? group = await unitOfWork.MessageRepository.GetMessageGroupAsync(request.GroupName, cancellationToken);
        return group is null
            ? Result<GroupDto?>.Success(null)
            : (Result<GroupDto?>)new GroupDto()
            {
                Name = group.Name,
                Connections = group.Connections.Select(x => new ConnectionDto(x.ConnectionId, x.UserId))
            };
    }
}

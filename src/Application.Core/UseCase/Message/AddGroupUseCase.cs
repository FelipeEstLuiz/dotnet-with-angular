using Application.Core.Model.Message;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class AddGroupUseCase(IMessageRepository messageRepository) : IRequestHandler<AddGroupModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddGroupModel request, CancellationToken cancellationToken = default)
    {
        Group? group = await messageRepository.GetMessageGroupAsync(request.GroupName, cancellationToken);

        if (group is null)
        {
            group = new(request.GroupName);
            await messageRepository.AddGroupAsync(group, cancellationToken);
        }

        group.Connections.Add(new Connection(request.ConnectionId, request.UserId));

        return await messageRepository.SaveAllChangesAsync(cancellationToken);
    }
}

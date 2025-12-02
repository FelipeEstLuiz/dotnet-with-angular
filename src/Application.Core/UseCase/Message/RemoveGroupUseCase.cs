using Application.Core.Model.Message;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class RemoveGroupUseCase(IMessageRepository messageRepository) : IRequestHandler<RemoveGroupModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(RemoveGroupModel request, CancellationToken cancellationToken = default)
    {
        await messageRepository.RemoveConnectionAsync(request.ConnectionId, cancellationToken);
        return Result.IsSuccess();
    }
}

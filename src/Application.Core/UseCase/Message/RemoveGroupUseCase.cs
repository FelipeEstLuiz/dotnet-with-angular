using Application.Core.Model.Message;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class RemoveGroupUseCase(IUnitOfWork unitOfWork) : IRequestHandler<RemoveGroupModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(RemoveGroupModel request, CancellationToken cancellationToken = default)
    {
        await unitOfWork.MessageRepository.RemoveConnectionAsync(request.ConnectionId, cancellationToken);
        return Result.IsSuccess();
    }
}

using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class GetMessageThreadUseCase(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageThreadModel, Result<IEnumerable<MessageDto>>>
{
    public async Task<Result<IEnumerable<MessageDto>>> Handle(GetMessageThreadModel request, CancellationToken cancellationToken = default)
    {
        IEnumerable<Domain.Entities.Message> result = await unitOfWork.MessageRepository.GetMessagesThreadAsync(request.UserId, request.RecipientId, cancellationToken);

        if (unitOfWork.HasChanges()) await unitOfWork.SaveAllChangesAsync(cancellationToken);

        return Result.Success(result.Select(MessageDto.Map));
    }
}

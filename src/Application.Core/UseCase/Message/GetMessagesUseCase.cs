using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class GetMessagesUseCase(IUnitOfWork unitOfWork) : IRequestHandler<GetMessageModel, Result<IEnumerable<MessageDto>>>
{
    public async Task<Result<IEnumerable<MessageDto>>> Handle(GetMessageModel request, CancellationToken cancellationToken = default)
    {
        MessageParams messageParams = new()
        {
            OrderBy = "MessageSent",
            OrderAsc = false,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            UserId = request.UserId,
            Container = request.Container
        };

        return (await unitOfWork.MessageRepository.GetMessagesForUserAsync(messageParams, cancellationToken))
            .Map(result => result!.Select(MessageDto.Map));
    }
}

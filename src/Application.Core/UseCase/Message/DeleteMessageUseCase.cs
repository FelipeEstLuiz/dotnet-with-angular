using Application.Core.Model.Message;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class DeleteMessageUseCase(IMessageRepository messageRepository) : IRequestHandler<DeleteMessageModel, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteMessageModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.Message? message = await messageRepository.GetMessageAsync(request.MessageId, cancellationToken);

        if (message is null)
            return Result.IsFailure("Cannot delete this message");
        else if (message.SenderId != request.UserId && message.RecipientId != request.UserId)
            return Result.IsFailure("You cannot delete this message");

        if (message.SenderId == request.UserId) message.SenderDeleted = true;
        if (message.RecipientId == request.UserId) message.RecipientDeleted = true;

        if (message is { RecipientDeleted: true, SenderDeleted: true })
            messageRepository.Delete(message);

        return Result.Try(await messageRepository.SaveAllChangesAsync(cancellationToken), "Failed to delete the message");
    }
}

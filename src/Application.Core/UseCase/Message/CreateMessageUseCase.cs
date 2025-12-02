using Application.Core.DTO.Message;
using Application.Core.Model.Message;
using Application.Domain.Enums;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;

namespace Application.Core.UseCase.Message;

public class CreateMessageUseCase(IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageModel, Result<MessageDto>>
{
    public async Task<Result<MessageDto>> Handle(CreateMessageModel request, CancellationToken cancellationToken = default)
    {
        Domain.Entities.User? sender = await unitOfWork.UserRepository.GetByIdAsync(
           request.UserId,
           cancellationToken
        );

        Domain.Entities.User? recipient = await unitOfWork.UserRepository.GetByIdAsync(
           request.RecipientId,
           cancellationToken
        );

        if (sender is null || recipient is null)
            return Result.Failure<MessageDto>("User not found.", ResponseCodes.USER_NOT_FOUND);
        else if (recipient.Id == sender.Id)
            return Result.Failure<MessageDto>("You cannot send message to yourself");

        Domain.Entities.Message message = new()
        {
            Content = request.Content,
            RecipientId = recipient.Id,
            SenderId = sender.Id,
            DateRead = request.DateRead
        };

        await unitOfWork.MessageRepository.AddAsync(message, cancellationToken);

        return await unitOfWork.SaveAllChangesAsync(cancellationToken)
            ? Result.Success(MessageDto.Map(message))
            : Result.Failure<MessageDto>("Failed to send message");
    }
}

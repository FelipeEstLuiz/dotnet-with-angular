namespace Application.Domain.Interfaces.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    ILikesRepository LikesRepository { get; }
    Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken);
    bool HasChanges();
}

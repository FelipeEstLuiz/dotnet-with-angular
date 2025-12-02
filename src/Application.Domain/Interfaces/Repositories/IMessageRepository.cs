using Application.Domain.Entities;
using Application.Domain.Model;

namespace Application.Domain.Interfaces.Repositories;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken cancellationToken);
    void Delete(Message message);
    Task<Message?> GetMessageAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<List<Message>>> GetMessagesForUserAsync(MessageParams messageParams, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetMessagesThreadAsync(string currentUserId, string recipientId, CancellationToken cancellationToken);
    Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken);
    Task AddGroupAsync(Group group, CancellationToken cancellationToken);
    Task RemoveConnectionAsync(string connectionId, CancellationToken cancellationToken);
    Task<Connection?> GetConnectionAsync(string connectionId, CancellationToken cancellationToken);
    Task<Group?> GetMessageGroupAsync(string groupName, CancellationToken cancellationToken);
    Task<Group?> GetMessageGroupForConnectionAsync(string connectionId, CancellationToken cancellationToken);
}

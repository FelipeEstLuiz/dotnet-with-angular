using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class MessageRepository(ApplicationDbContext context) : IMessageRepository
{
    public async Task AddAsync(Message message, CancellationToken cancellationToken) => await context.Messages.AddAsync(message, cancellationToken);

    public void Delete(Message message) => context.Remove(message);

    public async Task<Message?> GetMessageAsync(Guid id, CancellationToken cancellationToken)
        => await context.Messages.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

    public async Task<Result<List<Message>>> GetMessagesForUserAsync(MessageParams messageParams, CancellationToken cancellationToken)
    {
        IQueryable<Message> query = context
            .Messages
            .Include(x => x.Sender)
            .Include(x => x.Recipient)
            .AsQueryable();

        query = messageParams.Container switch
        {
            "Outbox" => query.Where(x => x.SenderId == messageParams.UserId && !x.SenderDeleted),
            _ => query.Where(x => x.RecipientId == messageParams.UserId && !x.RecipientDeleted)
        };

        return await query.ApplyQueryOptionsAsync(messageParams, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetMessagesThreadAsync(int currentUserId, int recipientId, CancellationToken cancellationToken)
    {
        await context
            .Messages
            .Where(x => x.SenderId == recipientId && x.RecipientId == currentUserId && !x.DateRead.HasValue)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.DateRead, DateTime.UtcNow), cancellationToken: cancellationToken);

        return await context
            .Messages
            .Include(x => x.Sender)
            .Include(x => x.Recipient)
            .Where(x => (x.SenderId == recipientId && !x.RecipientDeleted && x.RecipientId == currentUserId)
                || (x.SenderId == currentUserId && !x.SenderDeleted && x.RecipientId == recipientId))
            .OrderBy(x => x.MessageSent)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken) > 0;
}

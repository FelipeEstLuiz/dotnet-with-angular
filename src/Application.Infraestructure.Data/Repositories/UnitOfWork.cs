using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IUserRepository? _userRepository;
    private IMessageRepository? _messageRepository;
    private ILikesRepository? _likesRepository;

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(context);

    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(context);

    public ILikesRepository LikesRepository => _likesRepository ??= new LikesRepository(context);

    public async Task<bool> SaveAllChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await context.SaveChangesAsync(cancellationToken: cancellationToken) > 0;
        }
        catch (DbUpdateException ex)
        {
#pragma warning disable S112
            throw new Exception("An error occured while saving changes", ex);
#pragma warning restore S112
        }
    }

    public bool HasChanges() => context.ChangeTracker.HasChanges();
}

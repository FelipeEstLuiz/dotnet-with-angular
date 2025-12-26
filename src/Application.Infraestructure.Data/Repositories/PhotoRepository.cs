using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Infraestructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.Infraestructure.Data.Repositories;

public class PhotoRepository(ApplicationDbContext context) : IPhotoRepository
{
    public async Task<Photo?> GetPhotoById(int id, CancellationToken cancellationToken) => await context
        .Photos
        .IgnoreQueryFilters()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<Photo>> GetUnapprovedPhotos(CancellationToken cancellationToken) => await context
        .Photos
        .IgnoreQueryFilters()
        .Include(x => x.User)
        .Where(p => !p.IsApproved)
        .ToListAsync(cancellationToken);

    public void RemovePhoto(Photo photo) => context.Photos.Remove(photo);
}

using Application.Domain.Entities;

namespace Application.Domain.Interfaces.Repositories;

public interface IPhotoRepository
{
    Task<Photo?> GetPhotoById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Photo>> GetUnapprovedPhotos(CancellationToken cancellationToken);
    void RemovePhoto(Photo photo);
}

using Application.Domain.Entities;
using Application.Domain.Model;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Application.Domain.Interfaces.Services;

public interface IPhotoService
{
    Task<Result<Photo>> AddPhotoAsync(IFormFile file);
    Task<Result<DeletionResult>> DeletePhotoAsync(string publicId);
}

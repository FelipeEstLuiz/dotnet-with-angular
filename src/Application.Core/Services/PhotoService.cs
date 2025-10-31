using Application.Core.Model;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Core.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    private readonly IAppLogger<PhotoService> _logger;

    public PhotoService(IOptions<CloudinarySettings> options, IAppLogger<PhotoService> logger)
    {
        Account acc = new(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);

        _logger = logger;
    }

    public async Task<Result<Photo>> AddPhotoAsync(IFormFile file)
    {
        try
        {
            if (file.Length == 0)
                return Result<Photo>.Failure("Image not send");

            using Stream stream = file.OpenReadStream();
            ImageUploadParams uploadParams = new()
            {
                File = new FileDescription(file.Name, stream),
                Transformation = new Transformation()
                    .Height(500)
                    .Width(500)
                    .Crop("fill")
                    .Gravity(Gravity.Face),
                Folder = "da-net9"
            };

            ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

            return result.Error is null
                ? (Result<Photo>)new Photo()
                {
                    Url = result.Url.AbsoluteUri,
                    PublicId = result.PublicId
                }
                : Result<Photo>.Failure(result.Error.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image.");
            return Result<Photo>.Failure("Error uploading image.");
        }
    }

    public async Task<Result<DeletionResult>> DeletePhotoAsync(string publicId)
    {
        try
        {
            DeletionParams deleteParams = new(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error to remove image.");
            return Result<DeletionResult>.Failure("Error to remove image.");
        }
    }
}

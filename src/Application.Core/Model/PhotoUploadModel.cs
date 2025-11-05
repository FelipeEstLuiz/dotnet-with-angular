using Microsoft.AspNetCore.Http;

namespace Application.Core.Model;

public record PhotoUploadModel(int UserId, IFormFile File);

using Microsoft.AspNetCore.Http;

namespace Application.Core.Model;

public record PhotoUploadModel(string UserId, IFormFile File);

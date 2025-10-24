using Microsoft.AspNetCore.Http;

namespace Application.Core.Model;

public record PhotoUploadModel(IFormFile File, string UserName);

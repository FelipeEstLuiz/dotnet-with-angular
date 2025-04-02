using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Application.Api.Util;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            try
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
            catch (Exception) { /* Nao travar o processo */ }
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        string? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        OpenApiInfo info = new()
        {
            Title = $"Api dotnet with angular - v{assemblyVersion}",
            Version = description.ApiVersion.ToString(),
            Description = ""
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}

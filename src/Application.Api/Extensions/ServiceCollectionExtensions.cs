using Application.Api.Controllers._Shared;
using Application.Api.Middleware;
using Application.Api.Util;
using Application.Core.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO.Compression;
using System.Reflection;

namespace Application.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMvc(this IServiceCollection services)
    {
        services.AddCors();

        services.AddMvc(config =>
        {
            config.EnableEndpointRouting = false;
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = false,
                    ProcessExtensionDataNames = false
                }
            };
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }

    public static IServiceCollection AddGlobalExceptionMiddleware(this IServiceCollection services)
        => services.AddTransient<GlobalExceptionHandlerMiddleware>();

    public static IServiceCollection AddCommunicationProtocol(this IServiceCollection services)
        => services.AddScoped<CommunicationProtocol>();

    public static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                    return [api.GroupName];
                else if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    return [controllerActionDescriptor.ControllerName];

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });

            options.DocInclusionPredicate((name, api) => true);

            //string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            // Resolve conflitos de nomes de endpoints com versionamento
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        return services;
    }
}

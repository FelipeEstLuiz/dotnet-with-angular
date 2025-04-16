using Application.Api.Controllers._Shared;
using Application.Api.Filter;
using Application.Api.Middleware;
using Application.Api.Util;
using Application.Domain.Util;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO.Compression;
using System.Reflection;

namespace Application.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureExtensions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddCommunicationProtocol()
            .ConfigureMvc()
            .ConfigureJwt(configuration)
            .AddSwagger()
            .AddCompression()
            .AddHttpContextAccessor()
            .AddVersioning()
            .AddGlobalExceptionMiddleware()
            .AddHttpClient()
            .AddApplicationServices(); 

        return services;
    }

    private static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(ClsGlobal.GetTokenKey(configuration)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        });

        return services;
    }

    private static IServiceCollection ConfigureMvc(this IServiceCollection services)
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

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddValidatorsFromAssemblies(assemblies);
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }

    private static IServiceCollection AddFilters(this IServiceCollection services)
       => services.AddScoped<CustomAuthorizationFilter>();

    private static IServiceCollection AddGlobalExceptionMiddleware(this IServiceCollection services)
        => services.AddTransient<GlobalExceptionHandlerMiddleware>();

    private static IServiceCollection AddCommunicationProtocol(this IServiceCollection services)
        => services.AddScoped<CommunicationProtocol>();

    private static IServiceCollection AddCompression(this IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        return services;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
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

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] " +
                "and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

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

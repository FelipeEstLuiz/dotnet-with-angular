using Application.Api.Extensions;
using Application.Api.Middleware;
using Application.Infraestructure.IOC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

Assembly? assembly = Assembly.GetEntryAssembly();
string? appName = assembly?.GetName().Name;
string? appVersion = assembly?.GetName()?.Version?.ToString();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
       builder =>
       {
           builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
       });
});

KeyValuePair<string, string?> serilogIdSession = builder.Configuration.GetSection("Serilog:WriteTo").AsEnumerable().FirstOrDefault(ss => ss.Key.Contains("Args:path"));

if (serilogIdSession.Key is not null)
{
    IConfigurationSection? serilogSection = builder.Configuration.GetSection(serilogIdSession.Key);

    if (serilogSection is not null)
        serilogSection.Value = Path.Combine(serilogSection.Value!, $"{appName}_.log");
}

// Logging
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Conditional(levt => Environment.UserInteractive, lsc => lsc.Console())
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Version", appVersion)
);

builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, CustomAuthResultHandler>();

builder.Services.ConfigureExtensions(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("SqlServerDb"));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseCommunicationProtocolMiddleware();

app.UseGlobalExceptionMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200")
);

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

// Descomentar ao iniciar a solução sem dados
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<Application.Infraestructure.Data.Context.ApplicationDbContext>();
        var userManager = services
            .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Application.Domain.Entities.User>>();
        await context.Database.MigrateAsync();
        await Application.Infraestructure.Data.SeedData.Seed.SeedUsers(userManager);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

await app.RunAsync();

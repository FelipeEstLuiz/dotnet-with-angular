using Application.Api.Extensions;
using Application.Api.Middleware;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.SeedData;
using Application.Infraestructure.IOC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

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

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthResultHandler>();

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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await Seed.SeedUsers(context);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

await app.RunAsync();

using Application.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services
    .AddCommunicationProtocol()
    .ConfigureMvc()
    .AddSwagger()
    .AddCompression()
    .AddHttpContextAccessor()
    .AddVersioning()
    .AddGlobalExceptionMiddleware()
    .AddHttpClient()
    .AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseGlobalExceptionMiddleware();
app.UseCommunicationProtocolMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCompression();

app.UseRouting()
    .UseEndpoints(r =>
    {
        r.MapControllers();
    });

app.Run();

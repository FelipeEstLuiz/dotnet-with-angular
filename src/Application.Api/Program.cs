using Application.Api.Extensions;
using Application.Infraestructure.IOC;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddInfrastructure();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

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

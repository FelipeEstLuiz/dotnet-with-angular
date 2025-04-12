using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Repositories;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests.Data;

public class UsuarioRepositoryTests
{
    private readonly TestServer _server;
    private readonly Faker<Usuario> _faker;

    public UsuarioRepositoryTests()
    {
        _server = new(new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
                services.AddScoped<IUsuarioRepository, UsuarioRepository>();
                services.AddLogging();
            })
            .Configure(app => { }));

        _faker = new Faker<Usuario>()
            .RuleFor(cmd => cmd.Id, f => Guid.NewGuid())
            .RuleFor(cmd => cmd.Email, f => f.Internet.Email())
            .RuleFor(cmd => cmd.UserName, f => f.Name.FullName())
            .RuleFor(cmd => cmd.PasswordHash, f => f.Internet.Password(8));
    }

    [Fact]
    public async Task InsertAsync_DeveRetornarSuccess_QuandoInsercaoForBemSucedida()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        Result<bool> result = await repository.InsertAsync(_faker.Generate(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, await context.Usuarios.CountAsync());
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoErroAoInserirUsuario()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        context.Dispose();

        Result<bool> result = await repository.InsertAsync(_faker.Generate(), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao inserir usuario", result.Errors);
    }


    [Fact]
    public async Task GetByEmailAsync_DeveRetornarUsuario_QuandoEmailExistir()
    {
        Usuario usuario = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        Result<Usuario?> result = await repository.GetByEmailAsync(usuario.Email, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(usuario.UserName, result.Data.UserName);
    }

    [Fact]
    public async Task GetByEmailAsync_DeveRetornarUsuario_QuandoErroAoConsultar()
    {
        Usuario usuario = _faker.Generate();
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        context.Dispose();

        Result<Usuario?> result = await repository.GetByEmailAsync(usuario.Email, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao obter usuario", result.Errors);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoIdExistir()
    {
        Usuario usuario = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        await context.Usuarios.AddAsync(usuario);
        await context.SaveChangesAsync();

        Result<Usuario?> result = await repository.GetByIdAsync(usuario.Id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(usuario.UserName, result.Data.UserName);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoErroAoConsultar()
    {
        Usuario usuario = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        context.Dispose();

        Result<Usuario?> result = await repository.GetByIdAsync(usuario.Id, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao obter usuario", result.Errors);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuarios_QuandoExistiremUsuarios()
    {
        Usuario usuario1 = _faker.Generate();
        Usuario usuario2 = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        await context.Usuarios.AddAsync(usuario1);
        await context.Usuarios.AddAsync(usuario2);
        await context.SaveChangesAsync();

        Result<IEnumerable<Usuario>> result = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuario_QuandoErroAoConsultar()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        context.Dispose();

        Result<IEnumerable<Usuario>> result = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao obter usuarios", result.Errors);
    }
}

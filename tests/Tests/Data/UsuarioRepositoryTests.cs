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
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        await context.Usuarios.AddAsync(_faker.Generate());
        await context.Usuarios.AddAsync(_faker.Generate());
        await context.SaveChangesAsync();

        Result<List<Usuario>> result = await repository.GetAllAsync(cancellationToken: CancellationToken.None);

        Assert.True(result.IsSuccess);
#pragma warning disable CS8602
        Assert.Equal(2, result.Data.Count);
#pragma warning restore CS8602

        Assert.Null(result.PaginaAtual);
        Assert.Null(result.TotalPaginas);
        Assert.Null(result.TotalItens);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuarios_QuandoExistiremUsuarios_ServerSide()
    {
        List<Usuario> usuarios = [];

        for (int i = 0; i < 12; i++)
            usuarios.Add(_faker.Generate());

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();

        Result<List<Usuario>> result = await repository.GetAllAsync(
            new QueryOptions()
            {
                Pagina = 1,
                TamanhoPagina = 10
            },
            cancellationToken: CancellationToken.None
        );

        Assert.True(result.IsSuccess);
#pragma warning disable CS8602
        Assert.Equal(10, result.Data.Count);
#pragma warning restore CS8602
        Assert.Equal(12, result.TotalItens);
        Assert.Equal(2, result.TotalPaginas);
        Assert.Equal(1, result.PaginaAtual);

        Assert.NotNull(result.PaginaAtual);
        Assert.NotNull(result.TotalPaginas);
        Assert.NotNull(result.TotalItens);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuario_QuandoErroAoConsultar()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ILogger<UsuarioRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<UsuarioRepository>>();
        UsuarioRepository repository = new(context, logger);

        context.Dispose();

        Result<List<Usuario>> result = await repository.GetAllAsync(cancellationToken: CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao obter usuarios", result.Errors);
    }
}

using Application.Core.Services;
using Application.Domain.Entities;
using Application.Domain.Interfaces.Repositories;
using Application.Domain.Interfaces.Services;
using Application.Domain.Model;
using Application.Infraestructure.Data.Context;
using Application.Infraestructure.Data.Repositories;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Fixtures;

namespace Tests.Systems.Data;

public class UsuarioRepositoryTests
{
    private readonly TestServer _server;
    private readonly Faker<User> _faker;

    public UsuarioRepositoryTests()
    {
        _server = new(new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
                services.AddLogging();
                services.AddHttpContextAccessor();
            })
            .Configure(app => { })
        );

        _faker = GenerateUser.FakerUser();
    }

    [Fact]
    public async Task InsertAsync_DeveRetornarSuccess_QuandoInsercaoForBemSucedida()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IAppLogger<UserRepository> logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UserRepository>>();
        UserRepository repository = new(context, logger);

        Result<bool> result = await repository.InsertAsync(_faker.Generate(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoErroAoInserirUsuario()
    {
        // Arrange
        FakeDbContext dbContext = new();
        IAppLogger<UserRepository> logger = Substitute.For<IAppLogger<UserRepository>>();
        UserRepository repository = new(dbContext, logger);

        // Act
        Result<bool> result = await repository.InsertAsync(_faker.Generate(), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Insert user error", result.Errors);
    }


    [Fact]
    public async Task GetByEmailAsync_DeveRetornarUsuario_QuandoEmailExistir()
    {
        User usuario = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IAppLogger<UserRepository> logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UserRepository>>();
        UserRepository repository = new(context, logger);

        await context.Users.AddAsync(usuario);
        await context.SaveChangesAsync();

        Result<User?> result = await repository.GetByEmailAsync(usuario.Email, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(usuario.UserName, result.Data.UserName);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoIdExistir()
    {
        User usuario = _faker.Generate();

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IAppLogger<UserRepository> logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UserRepository>>();
        UserRepository repository = new(context, logger);

        await context.Users.AddAsync(usuario);
        await context.SaveChangesAsync();

        Result<User?> result = await repository.GetByIdAsync(usuario.Id, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(usuario.UserName, result.Data.UserName);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuarios_QuandoExistiremUsuarios()
    {
        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IAppLogger<UserRepository> logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UserRepository>>();
        UserRepository repository = new(context, logger);

        await context.Users.AddAsync(_faker.Generate());
        await context.Users.AddAsync(_faker.Generate());
        await context.SaveChangesAsync();

        Result<List<User>> result = await repository.GetAllAsync(cancellationToken: CancellationToken.None);

        Assert.True(result.IsSuccess);
#pragma warning disable CS8602
        Assert.Equal(2, result.Data.Count);
#pragma warning restore CS8602
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarUsuarios_QuandoExistiremUsuarios_ServerSide()
    {
        List<User> usuarios = [];

        for (int i = 0; i < 12; i++)
            usuarios.Add(_faker.Generate());

        using IServiceScope scope = _server.Host.Services.CreateScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        IAppLogger<UserRepository> logger = scope.ServiceProvider.GetRequiredService<IAppLogger<UserRepository>>();
        UserRepository repository = new(context, logger);

        await context.Users.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();

        Result<List<User>> result = await repository.GetAllAsync(
            new QueryOptions()
            {
                PageNumber = 1,
                PageSize = 10
            },
            cancellationToken: CancellationToken.None
        );

        Assert.True(result.IsSuccess);
#pragma warning disable CS8602
        Assert.Equal(10, result.Data.Count);
#pragma warning restore CS8602
        Assert.Equal(12, result.TotalItems);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(10, result.PageSize);
    }

    public sealed class FakeDbContext : ApplicationDbContext
    {
        public FakeDbContext() : base(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("FakeDb").Options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new Exception("Erro ao salvar");
    }
}

using Application.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Application.Infraestructure.Data.Repositories;

public class DatabaseConnection(IConfiguration configuration) : IDatabaseConnection
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(configuration.GetConnectionString("PostgresDb"));
}

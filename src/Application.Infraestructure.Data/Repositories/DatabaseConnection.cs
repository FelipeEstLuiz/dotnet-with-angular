using Npgsql;
using System.Data;

namespace Application.Infraestructure.Data.Repositories;

public class DatabaseConnection(string? connectionString)
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);
}

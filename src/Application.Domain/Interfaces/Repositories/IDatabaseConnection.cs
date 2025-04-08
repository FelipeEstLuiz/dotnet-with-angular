using System.Data;

namespace Application.Domain.Interfaces.Repositories;

public interface IDatabaseConnection
{
    IDbConnection CreateConnection();
}

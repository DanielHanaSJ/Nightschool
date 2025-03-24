using System.Data;

namespace AcademyApp.Api.Database.Dapper;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

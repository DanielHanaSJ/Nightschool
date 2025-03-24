using System.Data;
using AcademyApp.Api.Models;
using Microsoft.Data.SqlClient;

namespace AcademyApp.Api.Database.Dapper;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private string _connectionString;

    public SqlConnectionFactory(ConnectionStrings connectionStrings)
    {
        _connectionString = connectionStrings.NightschoolDB;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();

        return connection;
    }
}

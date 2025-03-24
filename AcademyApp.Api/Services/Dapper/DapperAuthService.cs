using System;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Database.Dapper;
using AcademyApp.Api.Models;
using AcademyApp.Api.Models.Entities;
using Dapper;

namespace AcademyApp.Api.Services.Dapper;

public class DapperAuthService : IAuthService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IAuthHelper _authHelper;

    public DapperAuthService(IDbConnectionFactory dbConnectionFactory, IAuthHelper authHelper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _authHelper = authHelper;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    { 
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var sql = "SELECT Id, Username, PasswordHash, PasswordSalt FROM [Users] WHERE Username = @Username";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });

        if (user == null)
        {
            throw new NotFoundException(nameof(user), username);
        }

        var passwordHash = user.PasswordHash;
        var passwordSalt = user.PasswordSalt;

        if (!_authHelper.VerifyPasswordHash(password, passwordHash, passwordSalt))
        {
            throw new BadRequestException("Invalid password.");
        }

        return _authHelper.GenerateToken(user);
    }

    public async Task<string> RegisterAsync(string username, string password)
    {
        _authHelper.CreatePasswordHash(password, out var hash, out var salt);
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        // Check if the username already exists
        var existingUser = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM [Users] WHERE Username = @Username", new { Username = username });

        if (existingUser != null)
        {
            throw new BadRequestException("Username already exists.");
        }


        var sql = "INSERT INTO [Users] (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)";
        var parameters = new
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        var rowsAffected = await connection.ExecuteAsync(sql, parameters);

        if (rowsAffected == 0)
        {
            throw new Exception("User registration failed.");
        }

        var userId = await connection.QuerySingleAsync<int>("SELECT Id FROM [Users] WHERE Username = @Username", new { Username = username });

        return _authHelper.GenerateToken(new User { Username = username, Id = userId });
    }

}

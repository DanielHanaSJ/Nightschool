using System;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Database.EfCore;
using AcademyApp.Api.Models;
using AcademyApp.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademyApp.Api.Services.EfCore;

public class EfCoreAuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IAuthHelper _authHelper;

    public EfCoreAuthService(DataContext context, IAuthHelper authHelper)
    {
        _context = context;
        _authHelper = authHelper;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            throw new NotFoundException(nameof(user), username);
        }

        if (!_authHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new BadRequestException("Invalid password.");
        }

        return _authHelper.GenerateToken(user);
    }

    public Task<string> RegisterAsync(string username, string password)
    {
        _authHelper.CreatePasswordHash(password, out var hash, out var salt);

        var existingUser = _context.Users.FirstOrDefault(x => x.Username == username);

        if (existingUser != null)
        {
            throw new BadRequestException("Username already exists.");
        }

        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Task.FromResult(_authHelper.GenerateToken(user));
    }
}

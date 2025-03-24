using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Models;
using AcademyApp.Api.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AcademyApp.Api.Helpers;

public class AuthHelper : IAuthHelper
{

    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHelper(AppSettings appSettings, IHttpContextAccessor httpContextAccessor) 
    {
        _appSettings = appSettings;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public string GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            , new Claim(ClaimTypes.Name, user.Username)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(_appSettings.TokenSecret));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        return int.Parse(userIdClaim.Value);
    }
}

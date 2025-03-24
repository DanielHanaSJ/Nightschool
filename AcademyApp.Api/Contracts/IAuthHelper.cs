using System;
using AcademyApp.Api.Models.Entities;

namespace AcademyApp.Api.Contracts;

public interface IAuthHelper
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    string GenerateToken(User user);
    int GetUserId();
}

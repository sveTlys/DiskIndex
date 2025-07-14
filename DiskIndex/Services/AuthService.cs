namespace DiskIndex.Services;

using Microsoft.AspNetCore.Identity;
using DiskIndex.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Xml;
using System.Linq.Dynamic.Core;

public class AuthService : IAuthService
{
    private readonly FileDbContext _dbContext;
    private readonly PasswordHasher<UserAccount> _hasher = new();

    public AuthService(FileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return false;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

    public async Task<string> CreateUserAsync(string username, string role)
    {
        Guid generatedToken = Guid.NewGuid();
        var user = new UserAccount
        {
            Username = username,
            Role = role,
            Token = generatedToken.ToString()
        };
       
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return generatedToken.ToString();
    }

    public async Task<bool> TokenVerificationAndPasswordChange(string username, string token, string newPassword)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Token == token);
        if (user == null) return false;

        user.PasswordHash = _hasher.HashPassword(user, newPassword);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}


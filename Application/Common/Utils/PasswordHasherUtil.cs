using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Utils;

public static class PasswordHasherUtil
{
    private static readonly PasswordHasher<User?> Hasher = new();

    public static string GetHashedPassword(string psw)
    {
        return Hasher.HashPassword(null, psw);
    } 
    
    public static bool VerifyPassword(string password, string passwordHash)
    {
        var result = Hasher.VerifyHashedPassword(null, passwordHash, password);
        return result != PasswordVerificationResult.Failed;
    }
}

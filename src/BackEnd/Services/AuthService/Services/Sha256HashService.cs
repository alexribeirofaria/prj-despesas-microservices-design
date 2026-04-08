using AuthService.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services;

public class Sha256HashService : IHashService
{
    public string Compute(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return string.Concat(bytes.Select(b => b.ToString("x2")));
    }
}
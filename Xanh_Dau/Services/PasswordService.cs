using System.Security.Cryptography;
using System.Text;

namespace Xanh_Dau.Services;

public class PasswordService
{
    private static readonly string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string Digits = "0123456789";
    private static readonly int length = 5;

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var hashedProvidedPassword = HashPassword(providedPassword);

        return hashedPassword.Equals(hashedProvidedPassword, StringComparison.OrdinalIgnoreCase);
    }

    public string GeneratePassword()
    {
        var random = new Random();

        var allCharacters = LowercaseLetters + UppercaseLetters + Digits;

        var password = new string(Enumerable.Repeat(allCharacters, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());

        return password;
    }
}
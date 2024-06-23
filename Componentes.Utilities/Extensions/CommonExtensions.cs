using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Componentes.Utilities.Extensions;

public static class CommonExtensions
{
    private static T? ToJsonObject<T>(this string source)
        => JsonConvert.DeserializeObject<T>(source);

    public static string ToJsonString(this object source)
        => JsonConvert.SerializeObject(source);

    public static bool IsValidEmail(this string text)
    {
        var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10})+)$");
        var match = emailRegex.Match(text);

        return match.Success;
    }

    public static string EncryptPass(string encrypt)
    {
        try
        {
            var result = BCrypt.Net.BCrypt.HashPassword(encrypt);

            return result;
        }
        catch (HashInformationException e)
        {
            Console.WriteLine(e.Message);

            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            throw;
        }
    }

    public static bool ValidatePass(string? hash, string password)
    {
        try
        {
            var result = BCrypt.Net.BCrypt.Verify(password, hash);

            return result;
        }
        catch (HashInformationException e)
        {
            Console.WriteLine(e.Message);

            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            throw;
        }
    }

    public static string GenerateJwToken(string issuer, string audience)
    {
        var rsa = RSA.Create();
        var privateKey = rsa.ExportParameters(true);

        var credentials = new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();
        var tokenString = handler.WriteToken(token);

        return tokenString;
    }

    public static async Task<bool> ValidateJwToken(string tokenString, string issuer, string audience)
    {
        var rsa = RSA.Create();
        var publicKey = rsa.ExportParameters(false);
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters()
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new RsaSecurityKey(publicKey)
        };

        var principal = await handler.ValidateTokenAsync(tokenString, validationParameters);

        return principal.IsValid;
    }
}
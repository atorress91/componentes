using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Componentes.Models.Requests.AuthRequest;
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

    public static string GenerateJwtToken(JwTokenRequest tokenRequest)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenRequest.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userRole = tokenRequest.User.UserRoleAssignments.FirstOrDefault();
        var roleId = userRole?.RoleId ?? 0;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, tokenRequest.User.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("name",tokenRequest.User.Name!),
            new Claim("lastName",tokenRequest.User.LastName!),
            new Claim("userId", tokenRequest.User.UserId.ToString()),
            new Claim("roleId", roleId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: tokenRequest.Issuer,
            audience: tokenRequest.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
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
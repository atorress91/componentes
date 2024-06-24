namespace Componentes.Core.Middlewares;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Utilities.Extensions;
using Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Jwt _jwtConfig;

    public JwtMiddleware(RequestDelegate next, IOptions<ApplicationConfiguration> appConfig)
    {
        _next = next;
        _jwtConfig = appConfig.Value.JwtConfig!;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var isValid = await CommonExtensions.ValidateJwToken(token, _jwtConfig.Issuer, _jwtConfig.Audience);
            if (isValid)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

                var identity = new ClaimsIdentity(claims, "jwt");
                context.User = new ClaimsPrincipal(identity);
            }
            else
            {
                var invalidClaim = new Claim("TokenStatus", "Invalid");
                var identity = new ClaimsIdentity(new[] { invalidClaim }, "jwt");
                context.User = new ClaimsPrincipal(identity);
            }
        }
        catch
        {
            var errorClaim = new Claim("TokenStatus", "Error");
            var identity = new ClaimsIdentity(new[] { errorClaim }, "jwt");
            context.User = new ClaimsPrincipal(identity);
        }
    }
}
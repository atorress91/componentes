using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Componentes.Models.Configuration;
using Componentes.Utilities.Extensions;

namespace Componentes.Core.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Jwt _jwtConfig;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IOptions<ApplicationConfiguration> appConfig,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _jwtConfig = appConfig.Value.JwtConfig!;
            _logger = logger;
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
                _logger.LogInformation("Token found, attaching user to context.");
                await AttachUserToContext(context, token);
            }
            else
            {
                _logger.LogWarning("No token found, request is unauthorized.");
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var isValid = await CommonExtensions.ValidateJwToken(
                    token,
                    _jwtConfig.Issuer,
                    _jwtConfig.Audience,
                    _jwtConfig.Key
                );

                if (isValid)
                {
                    _logger.LogInformation("Token is valid.");
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var claims = jwtToken.Claims;

                    var identity = new ClaimsIdentity(claims, "jwt");
                    context.User = new ClaimsPrincipal(identity);
                }
                else
                {
                    _logger.LogWarning("Invalid token.");
                    var invalidClaim = new Claim("TokenStatus", "Invalid");
                    var identity = new ClaimsIdentity(new[] { invalidClaim }, "jwt");
                    context.User = new ClaimsPrincipal(identity);
                    _logger.LogWarning(
                        "Unauthorized access attempt with invalid token from IP: {IpAddress}",
                        context.Connection.RemoteIpAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing token");
                var errorClaim = new Claim("TokenStatus", "Error");
                var identity = new ClaimsIdentity(new[] { errorClaim }, "jwt");
                context.User = new ClaimsPrincipal(identity);
            }
        }
    }
}
using System.Reflection;
using System.Text;
using Componentes.Core.Mapper;
using Componentes.Core.Services;
using Componentes.Core.Services.IServices;
using Componentes.Data.Database;
using Componentes.Data.Repositories;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.Configuration;
using Componentes.Core.Logger;
using Componentes.Core.Provider;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Componentes.Ioc;

public static class IocExtension
{
    public static void IocInjectAllDependencies(this IServiceCollection services, string[]? args = null)
    {
        InjectSwagger(services);
        InjectConfiguration(services);
        InjectAuthentication(services);
        InjectControllersAndDocumentation(services);
        InjectSingletonsAndFactories(services);
        InjectLogging(services);
        InjectDataBases(services);
        InjectRepositories(services);
        InjectServices(services);
        InjectValidators(services);
        InjectPackages(services);
        InjectAdapters(services);
        InjectCustomLogger(services);
    }

    private static void InjectAuthentication(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var jwtConfig = configuration.GetSection("AppSettings:JwtConfig");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtConfig["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }

    private static void InjectConfiguration(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var env = serviceProvider.GetRequiredService<IHostEnvironment>();
        var lowercaseEnvironment = env.EnvironmentName.ToLower();
        var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
        var builder = new ConfigurationBuilder()
            .SetBasePath(executableLocation)
            .AddJsonFile($"appsettings.{lowercaseEnvironment}.json", false, true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        var appSettingsSection = configuration.GetSection("AppSettings");

        services.Configure<ApplicationConfiguration>(appSettingsSection);
        services.AddSingleton(configuration);
    }

    private static void InjectSingletonsAndFactories(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }

    private static void InjectLogging(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        var lowerCaseEnvironment = env.EnvironmentName.ToLower();

        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddProvider(new FileLoggerProvider(new FileLogger("app_log.txt")));
        });

        Console.WriteLine($"IocLoggingRegister -> FileLogger configured");
    }

    public static void InjectSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proyecto Componentes Service", Version = "v1" });
        });

    private static void InjectDataBases(IServiceCollection services)
    {
        var appConfig = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationConfiguration>>()
            .Value;

        var connectionString = appConfig.ConnectionStrings?.SqlServerConnection;

        services.AddDbContext<ProyectoComponentesContext>(options => { options.UseSqlServer(connectionString); });
    }

    private static void InjectControllersAndDocumentation(IServiceCollection services, int majorVersion = 1,
        int minorVersion = 0)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/plain" });
        });
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
    }

    private static void InjectAdapters(IServiceCollection services)
    {
    }

    private static void InjectRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleAssignmentRepository, UserRoleAssignmentRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    private static void InjectServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
    }

    private static void InjectValidators(IServiceCollection services)
    {
    }

    private static void InjectPackages(IServiceCollection services)
        => services.AddAutoMapper(x => { x.AddProfile(new MapperProfile()); });

    private static void InjectCustomLogger(IServiceCollection services)
    {
        services.AddSingleton<FileLogger>(provider => new FileLogger("app_log.txt"));
    }
}
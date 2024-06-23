using System.Reflection;
using Componentes.Core.Mapper;
using Componentes.Data.Database;
using Componentes.Data.Repositories;
using Componentes.Data.Repositories.IRepositories;
using Componentes.Models.Configuration;
using FluentValidation.AspNetCore;
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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

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
    }

    private static void InjectAuthentication(IServiceCollection services)
        => services.AddAuthentication().AddJwtBearer();

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
            config.AddNLog($"nlog.{lowerCaseEnvironment}.config");
        });

        Console.WriteLine($"IocLoggingRegister -> nlog.{lowerCaseEnvironment}.config");
    }


    public static void InjectSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountService", Version = "v1" });
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
                    builder.WithOrigins("https://localhost:4200/")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true);
                });
        });
    }

    private static void InjectAdapters(IServiceCollection services)
    {
    }

    private static void InjectRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void InjectServices(IServiceCollection services)
    {
    }

    private static void InjectValidators(IServiceCollection services)
    {
    }

    private static void InjectPackages(IServiceCollection services)
        => services.AddAutoMapper(x => { x.AddProfile(new MapperProfile()); });
}
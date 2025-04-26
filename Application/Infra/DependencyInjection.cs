using System.Text;
using Application.Common.Behaviours;
using Application.Common.Interfaces.Helpers;
using Application.Infra.Context;
using Application.Infra.Services.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Infra;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
    }

    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddHttpContextAccessor();
    }

    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();
        services.AddScoped(_ => new DatabaseContextFactory().CreateDbContext());

        using var serviceProvider = services.BuildServiceProvider();
        using var context = serviceProvider.GetService<DatabaseContext>();

        context?.Database.Migrate();
    }

    public static void AddSwagger(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddSwaggerGen(c => c.LoadOpenApiOptions())
            .AddAuthentication(o => o.LoadAuthenticationOptions())
            .AddJwtBearer(o => o.LoadJwtBearerOptions(config));
    }

    private static void LoadOpenApiOptions(this SwaggerGenOptions options)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        };

        var securityReq = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        };

        var contact = new OpenApiContact() { Name = "Projekta API" };

        var info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Projekta API",
            Description = "API designed to manage the Projekta application.",
            Contact = contact,
        };

        options.SwaggerDoc("v1", info);
        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(securityReq);
    }

    private static void LoadAuthenticationOptions(this AuthenticationOptions o)
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void LoadJwtBearerOptions(this JwtBearerOptions o, IConfiguration config)
    {
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        var envKey = config["Jwt:Key"] ?? throw new ArgumentNullException(nameof(config));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(envKey));

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,
        };

        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
    }
}

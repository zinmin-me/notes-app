using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesApp.Application.Services;
using NotesApp.Application.Validators;
using NotesApp.Core.Interfaces.Repositories;
using NotesApp.Core.Interfaces.Services;
using NotesApp.Infrastructure.Data;
using NotesApp.Infrastructure.Repositories;
using NotesApp.Infrastructure.Services;

namespace NotesApp.Api.Extensions;

/// <summary>
/// Extension methods for configuring dependency injection and services.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Data Access
        services.AddSingleton<DapperContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Business Logic
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INoteService, NoteService>();
        
        // Infrastructure
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        // Validation
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not found.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Set to true in production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero // No delay on token expiration
            };
        });

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "Notes Management System API", 
                Version = "v1",
                Description = "A production-ready Full Stack Notes Management System API built with ASP.NET Core."
            });

            // Configure JWT Authentication in Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                              "Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            
            // Allow adding XML comments (requires property GenerateDocumentationFile=true in csproj)
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", builder =>
            {
                var frontendUrls = configuration["FrontendUrl"] ?? "http://localhost:5173";
                var origins = frontendUrls.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(url => url.Trim())
                                          .ToArray();
                builder.WithOrigins(origins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        return services;
    }
}

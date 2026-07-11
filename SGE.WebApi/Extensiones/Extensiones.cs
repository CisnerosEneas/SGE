using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using SGE.Aplicacion.Usuarios;

namespace SGE.WebApi;

public static class Extensiones
{
    public static IServiceCollection AddAutorizacionJWT(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenService, JwtTokenProvider>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    )
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddManejoDeExcepciones(
        this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<ManejadorDeExcepcionesGlobales>();

        return services;
    }
}
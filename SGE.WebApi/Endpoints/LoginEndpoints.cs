using System.Security.Claims;
using SGE.Aplicacion.Usuarios.UseCases;
using SGE.Aplicacion.Usuarios.DTOs;
namespace SGE.WebApi;
public static class LoginEndpoint
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/login", (LoginRequest request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).WithTags("Autenticación").AllowAnonymous();;
    }
}
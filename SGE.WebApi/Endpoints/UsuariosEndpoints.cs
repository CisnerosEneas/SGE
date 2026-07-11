using System.Security.Claims;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Aplicacion.Usuarios.UseCases;

namespace SGE.WebApi;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var usuariosApi = app.MapGroup("/api/usuarios")
                            .WithTags("Gestión de Usuarios");
                            

        usuariosApi.MapGet("/", (
            ClaimsPrincipal user,
            ListarUsuariosUseCase useCase) =>
        {
            var idUsuario = user.ObtenerIdUsuario();

            var request = new ListarUsuariosRequest(idUsuario);

            var response = useCase.Ejecutar(request);

            return Results.Ok(response);

        }).RequireAuthorization();

        usuariosApi.MapPost("/", (
            RegistrarUsuarioRequest request,
            RegistrarUsuarioUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);

            return Results.Created("/api/usuarios", response);

        });

        usuariosApi.MapPut("/mis-datos", (
            ModificarMisDatosRequest request,
            ClaimsPrincipal user,
            ModificarMisDatosUseCase useCase) =>
        {
            var idUsuario = user.ObtenerIdUsuario();

            request = new ModificarMisDatosRequest(
                idUsuario,
                request.Nombre,
                request.CorreoElectronico,
                request.Contrasena);

            var response = useCase.Ejecutar(request);

            return Results.Ok(response);

        }).RequireAuthorization();

        usuariosApi.MapPut("/{id:guid}/permisos", (
            Guid id,
            ModificarPermisosUsuarioRequest request,
            ClaimsPrincipal user,
            ModificarPermisosUsuarioUseCase useCase) =>
        {
            var idUsuario = user.ObtenerIdUsuario();

            request = new ModificarPermisosUsuarioRequest(
                id,
                request.Permisos,
                idUsuario);

            var response = useCase.Ejecutar(request);

            return Results.Ok(response);

        }).RequireAuthorization();

        usuariosApi.MapDelete("/{id:guid}", (
            Guid id,
            ClaimsPrincipal user,
            EliminarUsuarioUseCase useCase) =>
        {
            var idUsuario = user.ObtenerIdUsuario();

            var request = new EliminarUsuarioRequest(
                id,
                idUsuario);

            useCase.Ejecutar(request);

            return Results.NoContent();

        }).RequireAuthorization();
    }
}
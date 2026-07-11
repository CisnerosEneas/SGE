using System.Security.Claims;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Expedientes.UseCases;

namespace SGE.WebApi;

public static class ExpedientesEndpoints
{
    public static void MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        var expedientesApi = app.MapGroup("/api/expedientes")
                    .WithTags("Gestión de Expedientes")
                    .RequireAuthorization();

        expedientesApi.MapGet("/", (
        ListarExpedientesUseCase useCase) =>
        {
            var request = new ListarExpedientesRequest();
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        expedientesApi.MapPost("/", (
            AgregarExpedienteRequest request,
            ClaimsPrincipal user,
            AgregarExpedienteUseCase useCase) =>
        {
            var requestUseCase = new AgregarExpedienteRequest(
                request.TextoCaratula,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(requestUseCase);

            return Results.Created($"/api/expedientes/{response.IdExpediente}", response);
        });

        expedientesApi.MapPut("/{id:guid}", (
            Guid id,
            ModificarCaratulaExpedienteRequest request,
            ClaimsPrincipal user,
            ModificarCaratulaExpedienteUseCase useCase) =>
        {
            var requestUseCase = new ModificarCaratulaExpedienteRequest(
                id,
                request.NuevaCaratula,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(requestUseCase);

            return Results.Ok(response);
        });

        expedientesApi.MapPatch("/{id:guid}/estado", (
            Guid id,
            CambiarEstadoExpedienteRequest request,
            ClaimsPrincipal user,
            CambiarEstadoExpedienteUseCase useCase) =>
        {
            var requestUseCase = new CambiarEstadoExpedienteRequest(
                request.NuevoEstado,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(id, requestUseCase);

            return Results.Ok(response);
        });

        expedientesApi.MapDelete("/{id:guid}", (
            Guid id,
            ClaimsPrincipal user,
            EliminarExpedienteUseCase useCase) =>
        {
            var request = new EliminarExpedienteRequest(
                id,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(request);

            return Results.NoContent();
        });
    }
}
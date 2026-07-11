using System.Security.Claims;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.UseCases;

namespace SGE.WebApi;

public static class TramitesEndpoints
{
    public static void MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {
        var tramitesApi = app.MapGroup("/api/tramites")
                    .WithTags("Gestión de Trámites")
                    .RequireAuthorization();

        tramitesApi.MapGet("/expediente/{expedienteId:guid}", (
            Guid expedienteId,
            ListarTramitesPorExpedienteUseCase useCase) =>
        {
            var request = new ListarTramitesPorExpedienteRequest(expedienteId);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        });

        tramitesApi.MapPost("/", (
            AgregarTramiteRequest body,
            ClaimsPrincipal user,
            AgregarTramiteUseCase useCase) =>
        {
            var request = new AgregarTramiteRequest(
                body.ExpedienteId,
                body.Etiqueta,
                body.Contenido,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(request);

            return Results.Created($"/api/tramites/{response.Id}", response);
        });

        tramitesApi.MapPut("/{id:guid}", (
            Guid id,
            ModificarTramiteRequest body,
            ClaimsPrincipal user,
            ModificarTramiteUseCase useCase) =>
        {
            var request = new ModificarTramiteRequest(
                id,
                body.Etiqueta,
                body.Contenido,
                user.ObtenerIdUsuario()
            );

            var response = useCase.Ejecutar(request);

            return Results.Ok(response);
        });

        tramitesApi.MapDelete("/{id:guid}/expediente/{expedienteId:guid}", (
            Guid id,
            Guid expedienteId,
            ClaimsPrincipal user,
            EliminarTramiteUseCase useCase) =>
        {
            var request = new EliminarTramiteRequest(
                id,
                expedienteId,
                user.ObtenerIdUsuario()
            );

            useCase.Ejecutar(request);

            return Results.NoContent();
        });
    }
}
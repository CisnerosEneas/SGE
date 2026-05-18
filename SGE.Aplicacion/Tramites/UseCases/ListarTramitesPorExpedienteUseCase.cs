using System;
using SGE.Aplicacion.Tramites.DTOs;
namespace SGE.Aplicacion.Tramites;

public class ListarTramitesPorExpedienteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;

    public ListarTramitesPorExpedienteUseCase(ITramiteRepository tramiteRepository)
    {
        _tramiteRepository = tramiteRepository;
    }

    public IEnumerable<ListarTramitesPorExpedienteResponse> Ejecutar(ListarTramitesPorExpedienteRequest request)
    {
        var tramites = _tramiteRepository.ObtenerPorExpedienteId( request.ExpedienteId);
        return tramites.Select(t => new ListarTramitesPorExpedienteResponse( t.Id, t.ExpedienteId, t.Etiqueta, t.Contenido.Texto, t.FechaCreacion, t.FechaUltimaModificacion, t.UsuarioUltimoCambio));
    }
}
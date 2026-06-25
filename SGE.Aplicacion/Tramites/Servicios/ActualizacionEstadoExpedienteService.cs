using System;
using System.Linq;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites.Services;

public class ActualizacionEstadoExpedienteService(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
{
    public void ActualizarEstadoExpediente(Guid expedienteId, Guid idUsuario)
    {
        var expediente = expedienteRepository.ObtenerPorId(expedienteId);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        var tramites = tramiteRepository.ObtenerPorExpedienteId(expedienteId);
        var ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();
        EtiquetaTramite? ultimaEtiqueta = ultimoTramite?.Etiqueta;
        bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, idUsuario);
        if (cambio)
            expedienteRepository.Modificar(expediente);
    }
}
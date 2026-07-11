using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Usuarios;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class CambiarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public CambiarEstadoExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public CambiarEstadoExpedienteResponse Ejecutar(Guid idExpediente, CambiarEstadoExpedienteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion);
        if (!autorizado)
            throw new AutorizacionException();
        var expediente = _expedienteRepository.ObtenerPorId(idExpediente);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        bool actualizado = expediente.CambiarEstado(request.NuevoEstado, request.IdUsuario);
        if (actualizado)
        {
            _expedienteRepository.Modificar(expediente);
            _unidadDeTrabajo.Guardar();
        }
        return new CambiarEstadoExpedienteResponse(actualizado);
    }
}
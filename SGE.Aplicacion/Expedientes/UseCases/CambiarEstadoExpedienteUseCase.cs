using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class CambiarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;

    public CambiarEstadoExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion);
        if (!autorizado)
            throw new AutorizacionException();
        var expediente = _expedienteRepository.ObtenerPorId(request.IdExpediente);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        bool actualizado = expediente.CambiarEstado(request.NuevoEstado, request.IdUsuario);
        if (actualizado)
            _expedienteRepository.Modificar(expediente);
        return new CambiarEstadoExpedienteResponse(actualizado);
    }
}
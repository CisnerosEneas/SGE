using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Usuarios;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class AgregarExpedienteUseCase
{

    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public AgregarExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta);
        if (!autorizado)
            throw new AutorizacionException();
        var caratula = new Caratula(request.TextoCaratula);
        var expediente = new Expediente(caratula, request.IdUsuario);
        _expedienteRepository.Agregar(expediente);
        _unidadDeTrabajo.Guardar();
        return new AgregarExpedienteResponse(expediente.Id);
    }
}
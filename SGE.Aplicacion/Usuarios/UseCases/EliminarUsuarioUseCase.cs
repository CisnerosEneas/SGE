using System;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class EliminarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarUsuarioResponse Ejecutar(EliminarUsuarioRequest request)
    {
        var usuarioEjecutor = _usuarioRepository.ObtenerPorId(request.UsuarioEjecutor);
        if (usuarioEjecutor is null || !usuarioEjecutor.EsAdministrador)
            throw new Aplicacion.Autorizacion.AutorizacionException();

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuario);
        if (usuario is null)
            throw new EntidadNoEncontradaException("Usuario no encontrado");

        _usuarioRepository.Eliminar(usuario.Id);
        _unidadDeTrabajo.Guardar();
        return new EliminarUsuarioResponse(true);
    }
}

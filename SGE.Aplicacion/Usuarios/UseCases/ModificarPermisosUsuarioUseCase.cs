using System;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarPermisosUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarPermisosUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarPermisosUsuarioResponse Ejecutar(ModificarPermisosUsuarioRequest request)
    {
        var usuarioEjecutor = _usuarioRepository.ObtenerPorId(request.UsuarioEjecutor);
        if (usuarioEjecutor is null || !usuarioEjecutor.EsAdministrador)
            throw new Aplicacion.Autorizacion.AutorizacionException();

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuario);
        if (usuario is null)
            throw new EntidadNoEncontradaException("Usuario no encontrado");

        usuario.ActualizarPermisos(request.Permisos);
        _usuarioRepository.Modificar(usuario);
        _unidadDeTrabajo.Guardar();

        return new ModificarPermisosUsuarioResponse(true);
    }
}

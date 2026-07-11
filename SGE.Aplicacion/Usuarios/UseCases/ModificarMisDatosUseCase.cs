using System;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarMisDatosUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarMisDatosResponse Ejecutar(ModificarMisDatosRequest request)
    {
        if (request.IdUsuario == Guid.Empty)
            throw new Aplicacion.Comun.AplicacionException("Usuario inválido");

        var usuario = _usuarioRepository.ObtenerPorId(request.IdUsuario);
        if (usuario is null)
            throw new EntidadNoEncontradaException("Usuario no encontrado");

        usuario.ActualizarPerfil(request.Nombre, request.CorreoElectronico);
        usuario.CambiarContrasena(request.Contrasena);
        _usuarioRepository.Modificar(usuario);
        _unidadDeTrabajo.Guardar();

        return new ModificarMisDatosResponse(true);
    }
}

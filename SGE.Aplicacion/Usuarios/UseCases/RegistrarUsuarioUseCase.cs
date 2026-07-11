using System;
using System.Collections.Generic;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public RegistrarUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        var existente = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);
        if (existente is not null)
            throw new Aplicacion.Comun.AplicacionException("El correo electrónico ya está registrado");

        var usuario = Usuario.Crear(request.Nombre, request.CorreoElectronico, request.Contrasena, false, new List<Permiso>());
        _usuarioRepository.Agregar(usuario);
        _unidadDeTrabajo.Guardar();
        return new RegistrarUsuarioResponse(usuario.Id);
    }
}

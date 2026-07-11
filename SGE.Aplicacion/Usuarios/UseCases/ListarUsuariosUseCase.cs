using System;
using System.Linq;
using SGE.Aplicacion.Usuarios.DTOs;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public ListarUsuariosUseCase(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public ListarUsuariosResponse Ejecutar(ListarUsuariosRequest request)
    {
        var usuarioEjecutor = _usuarioRepository.ObtenerPorId(request.UsuarioEjecutor);
        if (usuarioEjecutor is null || !usuarioEjecutor.EsAdministrador)
            throw new Aplicacion.Autorizacion.AutorizacionException();

        var usuarios = _usuarioRepository.ObtenerTodos()
            .Select(u => new UsuarioDto(u.Id, u.Nombre, u.CorreoElectronico, u.EsAdministrador, u.Permisos))
            .ToList();

        return new ListarUsuariosResponse(usuarios);
    }
}

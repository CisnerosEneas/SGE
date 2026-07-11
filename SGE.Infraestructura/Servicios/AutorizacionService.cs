using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Servicios;

public class AutorizacionService : IAutorizacionService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AutorizacionService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public bool PoseeElPermiso(Guid idUsuario, SGE.Dominio.Usuarios.Permiso permiso)
    {
        var usuario = _usuarioRepository.ObtenerPorId(idUsuario);
        if (usuario is null)
            return false;

        if (permiso == SGE.Dominio.Usuarios.Permiso.TramiteBaja)
            return usuario.TienePermiso(SGE.Dominio.Usuarios.Permiso.TramiteBaja) || usuario.TienePermiso(SGE.Dominio.Usuarios.Permiso.ExpedienteBaja);

        return usuario.TienePermiso(permiso);
    }
}
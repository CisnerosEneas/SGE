using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record ModificarPermisosUsuarioRequest(Guid IdUsuario, IEnumerable<Permiso> Permisos, Guid UsuarioEjecutor);

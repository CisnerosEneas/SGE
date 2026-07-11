using System;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record EliminarUsuarioRequest(Guid IdUsuario, Guid UsuarioEjecutor);

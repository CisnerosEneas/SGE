using System.Collections.Generic;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record ListarUsuariosResponse(IReadOnlyCollection<UsuarioDto> Usuarios);

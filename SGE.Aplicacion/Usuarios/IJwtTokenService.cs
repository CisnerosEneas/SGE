using System;

namespace SGE.Aplicacion.Usuarios;

public interface IJwtTokenService
{
    string GenerarToken(Guid userId);
}

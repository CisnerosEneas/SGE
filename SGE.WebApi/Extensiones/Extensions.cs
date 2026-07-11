using System.Security.Claims;

namespace SGE.WebApi;

public static class ClaimsPrincipalExtensions
{
    public static Guid ObtenerIdUsuario(this ClaimsPrincipal user)
    {
        var id = user.FindFirst("ID")?.Value;

        if (string.IsNullOrWhiteSpace(id))
            throw new UnauthorizedAccessException("El token no contiene el ID del usuario.");

        return Guid.Parse(id);
    }
}
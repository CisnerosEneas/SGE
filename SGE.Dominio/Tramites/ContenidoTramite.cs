using System;

namespace SGE.Dominio.Tramites;

public record class ContenidoTramite
{
    public string Texto {get; }
    public ContenidoTramite(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new DominioException("El contenido no puede estar vacio ni ser nulo");
        Texto = texto;
    }
}

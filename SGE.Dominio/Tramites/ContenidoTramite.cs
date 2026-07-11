using System;

namespace SGE.Dominio.Tramites;

public record class ContenidoTramite
{
    public string Texto { get; set; }

    public ContenidoTramite()
    {
        Texto = string.Empty;
    }

    public ContenidoTramite(string texto)
    {
        Texto = string.IsNullOrWhiteSpace(texto)
            ? throw new DominioException("El contenido no puede estar vacio ni ser nulo")
            : texto;
    }
}

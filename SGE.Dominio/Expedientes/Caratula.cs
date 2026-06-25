using System;
namespace SGE.Dominio.Expedientes;

public record Caratula
{
    public string Texto {get; }
    public Caratula(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new DominioException("El texto no puede estar vacio ni ser nulo");
        Texto = texto;
    }
}

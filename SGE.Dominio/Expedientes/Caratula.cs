using System;
namespace SGE.Dominio.Expedientes;

public record Caratula
{
    public string Texto { get; set; }

    public Caratula()
    {
        Texto = string.Empty;
    }

    public Caratula(string texto)
    {
        Texto = string.IsNullOrWhiteSpace(texto)
            ? throw new DominioException("El texto no puede estar vacio ni ser nulo")
            : texto;
    }
}

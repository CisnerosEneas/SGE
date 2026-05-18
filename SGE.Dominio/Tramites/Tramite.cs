using System;

namespace SGE.Dominio.Tramites;

public class Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid idUsuarioCreador)
{
    public Guid Id {get; private set; } = Guid.NewGuid();
    public Guid ExpedienteId { get; private set; } = expedienteId;
    public EtiquetaTramite Etiqueta { get; private set; } = etiqueta;
    public ContenidoTramite Contenido { get; private set; } = contenido ?? throw new DominioException("El contenido no puede estar vacio ni ser nulo");
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;
    public Guid UsuarioUltimoCambio { get; private set; } = idUsuarioCreador;
    public DateTime FechaUltimaModificacion { get; private set; } = DateTime.Now;

    public void Modificar(EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid idUsuario)
    {
        Contenido = contenido ?? throw new DominioException("El contenido no puede estar vacio ni ser nulo");
        Etiqueta = etiqueta;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }
}
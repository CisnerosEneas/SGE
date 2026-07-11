using System;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }

    protected Tramite()
    {
        Contenido = new ContenidoTramite(string.Empty);
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = FechaCreacion;
    }

    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioUltimoCambio)
    {
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido ?? throw new DominioException("El contenido no puede estar vacio ni ser nulo");
        FechaCreacion = DateTime.Now;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        FechaUltimaModificacion = FechaCreacion;
    }

    public void Modificar(EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid idUsuario)
    {
        Contenido = contenido ?? throw new DominioException("El contenido no puede estar vacio ni ser nulo");
        Etiqueta = etiqueta;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }
}
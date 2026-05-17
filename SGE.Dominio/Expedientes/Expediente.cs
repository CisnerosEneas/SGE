using System;
using SGE.Dominio.Tramites;
namespace SGE.Dominio.Expedientes;

public class Expediente(Caratula c, Guid idUsuarioCreador)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Caratula Caratula { get; private set; } = c ?? throw new DominioException("La carátula es obligatoria");
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;
    public DateTime FechaUltimaModificacion { get; private set; } = DateTime.Now;
    public Guid UsuarioUltimoCambio { get; private set; } = idUsuarioCreador;
	public EstadoExpediente Estado { get; private set; } = EstadoExpediente.RecienIniciado;

	private void RegistrarModificacion(Guid idUsuarioModificador)
	{
		FechaUltimaModificacion = DateTime.Now;
		UsuarioUltimoCambio = idUsuarioModificador;
	}

	public void ModificarCaratula(Caratula caratulaModificada, Guid idUsuarioModificador)
	{
		Caratula = caratulaModificada ?? throw new DominioException("La carátula es obligatoria");
		RegistrarModificacion(idUsuarioModificador);
	}
	
	public bool ActualizarEstado(EtiquetaTramite? ultimaEtiqueta, Guid idUsuarioModificador)
	{
		EstadoExpediente estadoActualizado;
		if (ultimaEtiqueta is null)
		{
			estadoActualizado = EstadoExpediente.RecienIniciado;
		}
		else
		{
			estadoActualizado = ultimaEtiqueta switch
        	{
				EtiquetaTramite.Resolucion
					=> EstadoExpediente.ConResolucion,
				EtiquetaTramite.PaseAEstudio
					=> EstadoExpediente.ParaResolver,
				EtiquetaTramite.PaseAlArchivo
					=> EstadoExpediente.Finalizado,
				_ => Estado
        	};
		}
		if (estadoActualizado == Estado)
			return false;
		Estado = estadoActualizado;
		RegistrarModificacion(idUsuarioModificador);
		return true;
	}
}
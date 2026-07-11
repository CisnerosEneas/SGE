using System;
using SGE.Dominio.Tramites;
namespace SGE.Dominio.Expedientes;

public class Expediente
{
	public Guid Id { get; private set; }
	public Caratula Caratula { get; private set; }
	public DateTime FechaCreacion { get; private set; }
	public DateTime FechaUltimaModificacion { get; private set; }
	public Guid UsuarioUltimoCambio { get; private set; }
	public EstadoExpediente Estado { get; private set; }

	protected Expediente()
	{
		Caratula = new Caratula(string.Empty);
	}

	public Expediente(Caratula c, Guid idUsuarioCreador)
	{
		if (c is null)
			throw new DominioException("La carátula es obligatoria");
		if (idUsuarioCreador == Guid.Empty)
			throw new DominioException("El usuario creador no puede ser vacío");

		Id = Guid.NewGuid();
		Caratula = c;
		FechaCreacion = DateTime.Now;
		FechaUltimaModificacion = FechaCreacion;
		UsuarioUltimoCambio = idUsuarioCreador;
		Estado = EstadoExpediente.RecienIniciado;
	}

	private Expediente(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, EstadoExpediente estado)
	{
		Id = id;
		Caratula = caratula;
		FechaCreacion = fechaCreacion;
		FechaUltimaModificacion = fechaUltimaModificacion;
		UsuarioUltimoCambio = usuarioUltimoCambio;
		Estado = estado;
	}

	public static Expediente Reconstruir(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio, EstadoExpediente estado)
	{
		if (id == Guid.Empty)
			throw new DominioException("El id del expediente no puede ser vacío");
		if (caratula is null)
			throw new DominioException("La carátula es obligatoria");
		if (fechaCreacion == default)
			throw new DominioException("La fecha de creación es obligatoria");
		if (fechaUltimaModificacion == default)
			throw new DominioException("La fecha de última modificación es obligatoria");
		if (fechaUltimaModificacion < fechaCreacion)
			throw new DominioException("La fecha de última modificación no puede ser anterior a la fecha de creación");
		if (usuarioUltimoCambio == Guid.Empty)
			throw new DominioException("El usuario del último cambio no puede ser vacío");

		return new Expediente(id, caratula, fechaCreacion, fechaUltimaModificacion, usuarioUltimoCambio, estado);
	}

	private void RegistrarModificacion(Guid idUsuarioModificador)
	{
		if (idUsuarioModificador == Guid.Empty)
			throw new DominioException("El usuario que modifica no puede ser vacío");

		FechaUltimaModificacion = DateTime.Now;
		UsuarioUltimoCambio = idUsuarioModificador;
		if (FechaUltimaModificacion < FechaCreacion)
			FechaUltimaModificacion = FechaCreacion;
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

	public bool CambiarEstado(EstadoExpediente nuevoEstado, Guid idUsuarioModificador)
	{
		if (nuevoEstado == Estado)
			return false;
		Estado = nuevoEstado;
		RegistrarModificacion(idUsuarioModificador);
		return true;
	}
}
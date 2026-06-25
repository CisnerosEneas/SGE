using System;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.DTOs;

public record ModificarTramiteResponse(Guid Id, Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido, DateTime FechaUltimaModificacion);
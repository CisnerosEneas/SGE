using System;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.DTOs;

public record AgregarTramiteResponse( Guid Id, Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido, DateTime FechaCreacion);
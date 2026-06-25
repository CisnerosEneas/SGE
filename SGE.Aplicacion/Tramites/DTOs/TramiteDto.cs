using System;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.DTOs;

public record TramiteDto(Guid Id, Guid ExpedienteId, DateTime Fecha, string Contenido, string Etiqueta);
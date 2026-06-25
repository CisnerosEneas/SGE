using System;
namespace SGE.Aplicacion.Tramites.DTOs;

public record EliminarTramiteRequest(Guid TramiteId, Guid ExpedienteId, Guid IdUsuario);
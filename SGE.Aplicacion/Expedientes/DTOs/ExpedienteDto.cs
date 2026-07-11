using System;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Tramites.DTOs;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record ExpedienteDto(Guid Id, string Caratula, EstadoExpediente Estado, DateTime FechaCreacion, DateTime FechaUltimaModificacion, Guid UsuarioUltimoCambio, List<TramiteDto> Tramites);
using System;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record CambiarEstadoExpedienteRequest(EstadoExpediente NuevoEstado, Guid IdUsuario);
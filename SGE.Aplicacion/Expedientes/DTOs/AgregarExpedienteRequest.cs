using System;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record AgregarExpedienteRequest(string TextoCaratula, Guid IdUsuario);
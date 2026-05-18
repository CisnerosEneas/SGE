using System;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.DTOs;

public record AgregarTramiteRequest(Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido, Guid IdUsuario);
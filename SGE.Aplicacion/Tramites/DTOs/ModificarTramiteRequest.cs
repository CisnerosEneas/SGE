using System;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites.DTOs;

public record ModificarTramiteRequest(Guid TramiteId, EtiquetaTramite Etiqueta, string Contenido, Guid IdUsuario);
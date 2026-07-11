using System;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record ModificarMisDatosRequest(Guid IdUsuario, string Nombre, string CorreoElectronico, string Contrasena);

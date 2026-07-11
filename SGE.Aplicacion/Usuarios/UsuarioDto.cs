using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.DTOs;

public record UsuarioDto(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, IReadOnlyCollection<Permiso> Permisos);

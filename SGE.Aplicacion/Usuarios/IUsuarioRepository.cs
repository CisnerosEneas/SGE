using System;
using System.Collections.Generic;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    Usuario? ObtenerPorId(Guid id);
    Usuario? ObtenerPorCorreoElectronico(string correoElectronico);
    IEnumerable<Usuario> ObtenerTodos();
    void Modificar(Usuario usuario);
    void Eliminar(Guid id);
}

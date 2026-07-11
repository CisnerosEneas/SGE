using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Repositorios;

public class RepositorioUsuario : IUsuarioRepository
{
    private readonly SgeContext _context;

    public RepositorioUsuario(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Id == id);
    }

    public Usuario? ObtenerPorCorreoElectronico(string correoElectronico)
    {
        return _context.Usuarios.FirstOrDefault(u =>
            u.CorreoElectronico == correoElectronico);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }

    public void Modificar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
    }

    public void Eliminar(Guid id)
    {
        var usuario = ObtenerPorId(id);

        if (usuario != null)
            _context.Usuarios.Remove(usuario);
    }
}
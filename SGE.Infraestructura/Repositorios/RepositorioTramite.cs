using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura.Repositorios;

public class RepositorioTramite : ITramiteRepository
{
    private readonly SgeContext _context;

    public RepositorioTramite(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Tramite tramite)
    {
        _context.Tramites.Add(tramite);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return _context.Tramites.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        var idsEliminados = _context.ChangeTracker.Entries<Tramite>()
            .Where(entry => entry.State == EntityState.Deleted)
            .Select(entry => entry.Entity.Id)
            .ToHashSet();

        var tramitesLocal = _context.ChangeTracker.Entries<Tramite>()
            .Where(entry => entry.State != EntityState.Detached && entry.State != EntityState.Deleted && entry.Entity.ExpedienteId == expedienteId)
            .Select(entry => entry.Entity);

        var tramitesPersistidos = _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId && !idsEliminados.Contains(t.Id));

        return tramitesLocal
            .Concat(tramitesPersistidos)
            .GroupBy(t => t.Id)
            .Select(group => group.First())
            .OrderBy(t => t.FechaCreacion)
            .ToList();
    }

    public void Modificar(Tramite tramite)
    {
        _context.Tramites.Update(tramite);
    }

    public void Eliminar(Guid id)
    {
        var tramite = ObtenerPorId(id);

        if (tramite != null)
            _context.Tramites.Remove(tramite);
    }
}
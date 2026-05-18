using System;
using System.Collections.Generic;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Comun;
namespace SGE.Infraestructura.Repositorios;

public class RepositorioTramite : ITramiteRepository
{
    private readonly List<Tramite> _tramites = new List<Tramite>();

    public void Agregar(Tramite tramite)
    {
        _tramites.Add(tramite);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return _tramites.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(
        Guid expedienteId)
    {
        return _tramites.Where(t => t.ExpedienteId == expedienteId);
    }

    public void Modificar(Tramite tramite)
    {
        var existente = ObtenerPorId(tramite.Id);
        if (existente is null)
            throw new InfraestructuraException("Trámite no encontrado");
    }

    public void Eliminar(Guid id)
    {
        var tramite = ObtenerPorId(id);
        if (tramite is null)
            throw new InfraestructuraException("Trámite no encontrado");
        _tramites.Remove(tramite);
    }
}
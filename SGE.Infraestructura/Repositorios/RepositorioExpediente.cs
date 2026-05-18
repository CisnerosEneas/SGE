using System;
using System.Collections.Generic;
using SGE.Aplicacion.Expedientes;
using SGE.Infraestructura.Comun;
using SGE.Dominio.Expedientes;
namespace SGE.Infraestructura.Repositorios;

public class RepositorioExpediente : IExpedienteRepository
{
    private readonly List<Expediente> _expedientes = new List<Expediente>();

    public void Agregar(Expediente expediente)
    {
        _expedientes.Add(expediente);
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        return _expedientes.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        return _expedientes;
    }

    public void Modificar(Expediente expediente)
    {
        var existente = ObtenerPorId(expediente.Id);
        if (existente is null)
            throw new InfraestructuraException("Expediente no encontrado");
    }

    public void Eliminar(Guid id)
    {
        var expediente = ObtenerPorId(id);
        if (expediente is null)
            throw new InfraestructuraException("Expediente no encontrado");
        _expedientes.Remove(expediente);
    }
}
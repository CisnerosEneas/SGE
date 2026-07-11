using SGE.Aplicacion.Comun;

namespace SGE.Infraestructura.Comun;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;

    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context;
    }

    public void Guardar()
    {
        _context.SaveChanges();
    }
}
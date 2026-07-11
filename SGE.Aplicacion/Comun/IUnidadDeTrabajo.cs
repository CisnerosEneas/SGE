using System;
namespace SGE.Aplicacion.Comun;

public interface IUnidadDeTrabajo
{
    void Guardar(); // Confirma de forma atómica los cambios en la base de datos
}

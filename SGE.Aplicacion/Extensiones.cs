using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Tramites.UseCases;
using SGE.Aplicacion.Usuarios.UseCases;
using SGE.Aplicacion.Tramites.Services;
namespace SGE.Aplicacion;
public static class Extensiones
{
    public static IServiceCollection AddAplicacion(this IServiceCollection servicios)

    {
        // Casos de uso de expedientes
        servicios.AddScoped<AgregarExpedienteUseCase>();
        servicios.AddScoped<CambiarEstadoExpedienteUseCase>();
        servicios.AddScoped<EliminarExpedienteUseCase>();
        servicios.AddScoped<ListarExpedientesUseCase>();
        servicios.AddScoped<ModificarCaratulaExpedienteUseCase>();
        servicios.AddScoped<ActualizacionEstadoExpedienteService>();

        // Casos de uso de tramites
        servicios.AddScoped<AgregarTramiteUseCase>();
        servicios.AddScoped<EliminarTramiteUseCase>();
        servicios.AddScoped<ListarTramitesPorExpedienteUseCase>();
        servicios.AddScoped<ModificarTramiteUseCase>();
        servicios.AddScoped<ActualizacionEstadoExpedienteService>();

        // Casos de uso de usuarios
        servicios.AddScoped<EliminarUsuarioUseCase>();
        servicios.AddScoped<ListarUsuariosUseCase>();
        servicios.AddScoped<LoginUseCase>();
        servicios.AddScoped<ModificarMisDatosUseCase>();
        servicios.AddScoped<ModificarPermisosUsuarioUseCase>();
        servicios.AddScoped<RegistrarUsuarioUseCase>();
        return servicios;
    }
}
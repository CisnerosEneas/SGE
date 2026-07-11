using Microsoft.Extensions.DependencyInjection;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.Infraestructura.Servicios;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SGE.Infraestructura.Repositorios;
using SGE.Aplicacion.Comun;
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura;
public static class Extensiones
{
public static IServiceCollection AddInfraestructura(this IServiceCollection servicios,
IConfiguration configuration)

{
// A. Base de Datos. Extraemos la cadena de conexión del archivo appsettings.json
var connectionString = configuration.GetConnectionString("SgeDb");
servicios.AddDbContext<SgeContext>(opciones =>
opciones.UseSqlite(connectionString));
// B. Seguridad (Autorización)
servicios.AddScoped<IAutorizacionService, AutorizacionService>();
servicios.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
servicios.AddScoped<IExpedienteRepository, RepositorioExpediente>();
servicios.AddScoped<ITramiteRepository, RepositorioTramite>();
servicios.AddScoped<IUsuarioRepository, RepositorioUsuario>();
return servicios;
}
}
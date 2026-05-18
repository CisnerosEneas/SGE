using System;
using SGE.Infraestructura.Repositorios;
using SGE.Infraestructura.Servicios;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Consola;

public class Program
{
    public static void Main()
    {
        var expedienteRepo = new RepositorioExpediente();
        var tramiteRepo = new RepositorioTramite();
        var authService = new ServicioAutorizacion();
        var estadoService = new ActualizacionEstadoExpedienteService(expedienteRepo, tramiteRepo);

        var agregarExpedienteUC = new AgregarExpedienteUseCase(expedienteRepo, authService);
        var listarExpedientesUC = new ListarExpedientesUseCase(expedienteRepo);

        Console.WriteLine("=== Flujo: Camino Feliz ===");
        try
        {
            var usuario = Guid.NewGuid();
            var req = new AgregarExpedienteRequest("Caratula inicial", usuario);
            var resp = agregarExpedienteUC.Ejecutar(req);
            Console.WriteLine($"Expediente creado: {resp.IdExpediente}");

            // Agregar un trámite directamente al repositorio para simular cambio de estado
            var tramite = new Tramite(resp.IdExpediente, EtiquetaTramite.Resolucion, new ContenidoTramite("Contenido resolución"), usuario);
            tramiteRepo.Agregar(tramite);

            // Actualizar estado del expediente según últimos trámites
            estadoService.ActualizarEstadoExpediente(resp.IdExpediente, usuario);

            var listResp = listarExpedientesUC.Ejecutar();
            foreach (var e in listResp.Expedientes)
                Console.WriteLine($"Listado: {e.Id} - {e.Caratula} - Estado: {e.Estado}");
        }
        catch (SGE.Dominio.DominioException dex)
        {
            Console.WriteLine($"Dominio: {dex.Message}");
        }
        catch (SGE.Aplicacion.Autorizacion.AutorizacionException aex)
        {
            Console.WriteLine($"Autorización: {aex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general: {ex.Message}");
        }

        Console.WriteLine("\n=== Flujo: Camino de Error - Carátula vacía ===");
        try
        {
            // Provocar DominioException creando una carátula vacía
            var car = new Caratula("");
        }
        catch (SGE.Dominio.DominioException dex)
        {
            Console.WriteLine($"Dominio detectado: {dex.Message}");
        }

        Console.WriteLine("\n=== Flujo: Camino de Error - Autorización denegada ===");
        try
        {
            // Usar un servicio de autorización que niega permisos
            var authDenied = new DenyAutorizacionService();
            var agregarDenied = new AgregarExpedienteUseCase(expedienteRepo, authDenied);
            var req2 = new AgregarExpedienteRequest("Caratula segura", Guid.NewGuid());
            agregarDenied.Ejecutar(req2);
        }
        catch (SGE.Aplicacion.Autorizacion.AutorizacionException aex)
        {
            Console.WriteLine($"Autorización detectada: {aex.Message}");
        }

        Console.WriteLine("\nFin de la simulación.");
    }

    // Servicio de autorización temporal que niega permisos
    class DenyAutorizacionService : SGE.Aplicacion.Autorizacion.IAutorizacionService
    {
        public bool PoseeElPermiso(Guid idUsuario, SGE.Aplicacion.Autorizacion.Permiso permiso) => false;
    }
}

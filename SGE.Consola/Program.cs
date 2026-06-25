using System;
using SGE.Infraestructura.Repositorios;
using SGE.Infraestructura.Servicios;
using SGE.Aplicacion.Expedientes.UseCases;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Consola;

public class Program
{
    public static void Main()
    {
        var expedienteRepositorio = new RepositorioExpediente();
        var tramiteRepositorio = new RepositorioTramite();
        var servicioDeAutorizacion = new ServicioAutorizacion();
        var estadoService = new ActualizacionEstadoExpedienteService(expedienteRepositorio, tramiteRepositorio);
        var agregarExpedienteUC = new AgregarExpedienteUseCase(expedienteRepositorio, servicioDeAutorizacion);
        var listarExpedientesUC = new ListarExpedientesUseCase(expedienteRepositorio);

        // Crear un expediente y agregar tramites para simular el flujo normal
        Console.WriteLine("Simulacion de flujo normal:");
        try
        {
            Console.WriteLine("Creando expedientes y agregando tramites...");
            var usuario = Guid.NewGuid();
            var agregarExpediente = new AgregarExpedienteRequest("Caratula de testeo 1", usuario);
            var respuesta = agregarExpedienteUC.Ejecutar(agregarExpediente);
            Console.WriteLine($"Expediente creado con Guid: {respuesta.IdExpediente}");

            agregarExpediente = new AgregarExpedienteRequest("Caratula de testeo 2", usuario);
            respuesta = agregarExpedienteUC.Ejecutar(agregarExpediente);
            Console.WriteLine($"Expediente creado con Guid: {respuesta.IdExpediente}");

            var tramite = new Tramite(respuesta.IdExpediente, EtiquetaTramite.Resolucion, new ContenidoTramite("Contenido resolución"), usuario);
            tramiteRepositorio.Agregar(tramite);

            estadoService.ActualizarEstadoExpediente(respuesta.IdExpediente, usuario);

            var listRespuesta = listarExpedientesUC.Ejecutar();
            Console.WriteLine("\nListado de expedientes:");
            foreach (var e in listRespuesta.Expedientes)
                Console.WriteLine($"Guid expediente: {e.Id} - Caratula: {e.Caratula} - Estado: {e.Estado}");
        }
        catch (SGE.Dominio.DominioException ex)
        {
            Console.WriteLine($"Dominio: {ex.Message}");
        }
        catch (SGE.Aplicacion.Autorizacion.AutorizacionException ex)
        {
            Console.WriteLine($"Autorización: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general: {ex.Message}");
        }

        Console.WriteLine("\nSimulacion de flujo con excepciones:");
        // Agregar caratula vacia para probar validacion de dominio
        try
        {
            var car = new Caratula("");
        }
        catch (SGE.Dominio.DominioException ex)
        {
            Console.WriteLine($"Error de dominio: {ex.Message}");
        }

        // Probar denegacion de autorizacion
        try
        {
            var denegarAutorizacion = new DenegadorAutorizacionTest();
            var agregarDenegado = new AgregarExpedienteUseCase(expedienteRepositorio, denegarAutorizacion);
            var agregarExpediente = new AgregarExpedienteRequest("Caratula segura", Guid.NewGuid());
            agregarDenegado.Ejecutar(agregarExpediente);
        }
        catch (SGE.Aplicacion.Autorizacion.AutorizacionException ex)
        {
            Console.WriteLine($"Error de aplicacion: {ex.Message}");
        }
    }

    // Clase pivot para simular denegacion de permisos (Solo para testeos)
    class DenegadorAutorizacionTest : SGE.Aplicacion.Autorizacion.IAutorizacionService
    {
        public bool PoseeElPermiso(Guid idUsuario, SGE.Aplicacion.Autorizacion.Permiso permiso) => false;
    }
}

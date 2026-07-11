using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public static class SGESqlite
{
    public static void Inicializar(SgeContext context)
    {
        if (context.Database.EnsureCreated())
        {
            Console.WriteLine("Se creó base de datos");
            
            var connection = context.Database.GetDbConnection();
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA journal_mode=DELETE;";
                command.ExecuteNonQuery();
            }
            
            var admin = Usuario.Crear("Administrador", "admin@sge.com", "admin123", true, new List<Permiso>());
            var usuarioLectura = Usuario.Crear("Usuario Lectura", "lectura@sge.com", "lectura123", false, new List<Permiso>());
            var usuarioAlta = Usuario.Crear("Usuario alta", "alta@sge.com", "alta123", false, new List<Permiso>
            {
                Permiso.ExpedienteAlta,
                Permiso.TramiteAlta
            });
            var usuarioExpediente = Usuario.Crear("Usuario Expediente", "expediente@sge.com", "expediente123", false, new List<Permiso>
            {
                Permiso.ExpedienteAlta,
                Permiso.ExpedienteModificacion,
                Permiso.ExpedienteBaja
            });
            var usuarioTramite = Usuario.Crear("Usuario Tramite", "tramite@sge.com", "tramite123", false, new List<Permiso>
            {
                Permiso.TramiteAlta,
                Permiso.TramiteModificacion,
                Permiso.TramiteBaja
            });
            
            var usuarioMixto = Usuario.Crear("Usuario Mixto", "mixto@sge.com", "mixto123", false, new List<Permiso>
            {
                Permiso.ExpedienteAlta,
                Permiso.TramiteAlta,
                Permiso.TramiteModificacion
            });
            context.Usuarios.Add(admin);
            context.Usuarios.Add(usuarioLectura);
            context.Usuarios.Add(usuarioAlta);
            context.Usuarios.Add(usuarioExpediente);
            context.Usuarios.Add(usuarioTramite);
            context.Usuarios.Add(usuarioMixto);

            context.SaveChanges();
        }
    }
}

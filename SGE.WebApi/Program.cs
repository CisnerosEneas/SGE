using Scalar.AspNetCore;
using SGE.Aplicacion;
using SGE.Infraestructura;
using SGE.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddAplicacion()
    .AddInfraestructura(builder.Configuration)
    .AddAutorizacionJWT(builder.Configuration)
    .AddManejoDeExcepciones();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    SGESqlite.Inicializar(context);
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapLoginEndpoint();
app.MapUsuariosEndpoints();
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();

app.Run();
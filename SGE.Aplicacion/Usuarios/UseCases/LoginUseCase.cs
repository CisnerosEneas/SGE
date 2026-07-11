using System;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Usuarios.DTOs;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios.UseCases;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenService _jwtTokenService;
    public LoginUseCase(
    IUsuarioRepository usuarioRepository,
    IJwtTokenService jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenService = jwtTokenService;
    }
    public LoginResponse Ejecutar(LoginRequest request)
    {
        var usuario = _usuarioRepository.ObtenerPorCorreoElectronico(request.CorreoElectronico);

        if (usuario == null)
        {
            Console.WriteLine("Usuario NO encontrado.");
            throw new AutorizacionException("Credenciales inválidas");
        }

        var valida = usuario.VerificarContrasena(request.Contrasena);

        if (!valida)
            throw new AutorizacionException("Credenciales inválidas");

        var token = _jwtTokenService.GenerarToken(usuario.Id);
        return new LoginResponse(token);
    }
}

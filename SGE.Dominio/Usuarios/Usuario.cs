using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string CorreoElectronico { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdministrador { get; private set; }
    private readonly List<Permiso> _permisos;
    public IReadOnlyCollection<Permiso> Permisos => _permisos.AsReadOnly();

    private Usuario(Guid id, string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador, List<Permiso> permisos)
    {
        Id = id;
        Nombre = string.IsNullOrWhiteSpace(nombre) ? throw new DominioException("El nombre no puede estar vacio ni ser nulo") : nombre;
        CorreoElectronico = string.IsNullOrWhiteSpace(correoElectronico) ? throw new DominioException("El correo electrónico no puede estar vacio ni ser nulo") : correoElectronico;
        ContrasenaHash = string.IsNullOrWhiteSpace(contrasenaHash) ? throw new DominioException("La contraseña no puede estar vacia ni ser nula") : contrasenaHash;
        EsAdministrador = esAdministrador;
        _permisos = permisos != null
            ? new List<Permiso>(permisos)
            : throw new DominioException("Los permisos no pueden ser nulos");
    }

    public static Usuario Crear(string nombre, string correoElectronico, string contrasena, bool esAdministrador, List<Permiso>? permisos = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DominioException("El nombre no puede estar vacio ni ser nulo");
        if (string.IsNullOrWhiteSpace(correoElectronico)) throw new DominioException("El correo electrónico no puede estar vacio ni ser nulo");
        if (string.IsNullOrWhiteSpace(contrasena)) throw new DominioException("La contraseña no puede estar vacia ni ser nula");

        var hash = HashContrasena(contrasena);
        return new Usuario(Guid.NewGuid(), nombre, correoElectronico, hash, esAdministrador, permisos ?? new List<Permiso>());
    }

    public static Usuario Reconstruir(Guid id, string nombre, string correoElectronico, string contrasenaHash, bool esAdministrador, List<Permiso>? permisos)
    {
        if (id == Guid.Empty) throw new DominioException("Id inválido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DominioException("El nombre no puede estar vacio ni ser nulo");
        if (string.IsNullOrWhiteSpace(correoElectronico)) throw new DominioException("El correo electrónico no puede estar vacio ni ser nulo");
        if (string.IsNullOrWhiteSpace(contrasenaHash)) throw new DominioException("La contraseña hash no puede estar vacia ni ser nula");

        return new Usuario(id, nombre, correoElectronico, contrasenaHash, esAdministrador, permisos ?? new List<Permiso>());
    }

    private static string HashContrasena(string contrasena)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            System.Text.Encoding.UTF8.GetBytes(contrasena),
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public bool VerificarContrasena(string contrasena)
    {
        var parts = ContrasenaHash.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var expectedHash = Convert.FromBase64String(parts[1]);

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            System.Text.Encoding.UTF8.GetBytes(contrasena),
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    public void AgregarPermiso(Permiso permiso)
    {
        if (_permisos.Contains(permiso)) return;
        _permisos.Add(permiso);
    }

    public void QuitarPermiso(Permiso permiso)
    {
        if (!_permisos.Contains(permiso)) return;
        _permisos.Remove(permiso);
    }

    public void ActualizarPerfil(string nombre, string correoElectronico)
    {
        Nombre = string.IsNullOrWhiteSpace(nombre) ? throw new DominioException("El nombre no puede estar vacio ni ser nulo") : nombre;
        CorreoElectronico = string.IsNullOrWhiteSpace(correoElectronico) ? throw new DominioException("El correo electrónico no puede estar vacio ni ser nulo") : correoElectronico;
    }

    public void CambiarContrasena(string contrasena)
    {
        if (string.IsNullOrWhiteSpace(contrasena))
            throw new DominioException("La contraseña no puede estar vacia ni ser nula");
        ContrasenaHash = HashContrasena(contrasena);
    }

    public void ActualizarPermisos(IEnumerable<Permiso> permisos)
    {
        if (permisos == null) throw new DominioException("Los permisos no pueden ser nulos");
        _permisos.Clear();
        _permisos.AddRange(permisos);
    }

    public bool TienePermiso(Permiso permiso)
    {
        if (EsAdministrador) return true;
        return _permisos.Contains(permiso);
    }

}
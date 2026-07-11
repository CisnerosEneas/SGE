namespace SGE.Aplicacion.Comun;

public class AutorizacionException : AplicacionException
{
    public AutorizacionException()
        : base("No cuenta con la autorización necesaria")
    {
    }

    public AutorizacionException(string mensaje) : base(mensaje) { }

    public AutorizacionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
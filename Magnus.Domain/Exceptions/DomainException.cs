namespace Magnus.Domain.Exceptions
{
    /// <summary>
    /// Excepción base para errores del dominio (reglas de negocio).
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
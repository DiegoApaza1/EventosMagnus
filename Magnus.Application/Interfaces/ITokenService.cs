using Magnus.Domain.Entities;
using System.Threading.Tasks;

namespace Magnus.Application.Interfaces
{
    public interface ITokenService
    {
        // Método existente
        string CreateToken(Usuario user); 
        
        // NUEVO: Genera un token aleatorio, seguro y de un solo uso para el restablecimiento.
        string GeneratePasswordResetToken(); 
    }
}
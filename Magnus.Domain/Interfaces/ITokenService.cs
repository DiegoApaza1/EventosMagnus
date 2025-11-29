namespace Magnus.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string nombre, string email, IEnumerable<KeyValuePair<string,string>>? extraClaims = null, DateTime? nowUtc = null);
        string GeneratePasswordResetToken();
    }
}

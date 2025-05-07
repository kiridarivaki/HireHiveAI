namespace HireHive.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string email);
    }
}

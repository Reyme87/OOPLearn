namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email);
    }
}

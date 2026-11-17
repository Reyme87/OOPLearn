namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto request, CancellationToken cancellationToken);

        Task<AuthResponseDto> LoginAsync(LoginDto request, CancellationToken cancellationToken);
    }
}

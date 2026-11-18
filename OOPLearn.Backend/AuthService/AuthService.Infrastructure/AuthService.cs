using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain;

namespace AuthService.Infrastructure
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IAppDbContext _dbContext;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IAppDbContext dbContext)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterDto request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Не указан email.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Не указан пароль.");

            if (request.Password.Length < 8)
                throw new ArgumentException(
                    "Пароль должен содержать не менее 8 символов.");

            if (string.IsNullOrWhiteSpace(request.FirstName))
                throw new ArgumentException("Не указано имя.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                throw new ArgumentException("Не указана фамилия.");

            var existingUser = await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

            if (existingUser is not null)
                throw new InvalidOperationException(
                    "Пользователь с таким email уже существует.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim()
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreateAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

            if (user is null ||
                !_passwordHasher.VerifyPassword(
                    request.Password,
                    user.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                    "Неверный email или пароль.");
            }

            if (!user.IsActive)
                throw new UnauthorizedAccessException(
                    "Учётная запись заблокирована.");

            return CreateAuthResponse(user);
        }

        private AuthResponseDto CreateAuthResponse(User user)
        {
            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = _tokenService.GenerateAccessToken(
                    user.Id,
                    user.Email)
            };
        }
    }
}

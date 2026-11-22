using Auth;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using Grpc.Core;

namespace AuthServiceApp.Services
{
    public class AuthServiceApi : AuthGrpcService.AuthGrpcServiceBase
    {
        private readonly IAuthService _authService;

        public AuthServiceApi(IAuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _authService.RegisterAsync(
                    new RegisterDto
                    {
                        Email = request.Email,
                        Password = request.Password,
                        FirstName = request.FirstName,
                        LastName = request.LastName
                    },
                    context.CancellationToken);

                return MapResponse(result);
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.AlreadyExists, ex.Message));
            }
        }

        public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _authService.LoginAsync(
                    new LoginDto
                    {
                        Email = request.Email,
                        Password = request.Password
                    },
                    context.CancellationToken);

                return MapResponse(result);
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new RpcException(
                    new Status(StatusCode.Unauthenticated, ex.Message));
            }
        }

        private static AuthResponse MapResponse(AuthResponseDto result)
        {
            return new AuthResponse
            {
                UserId = result.Id.ToString(),
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                AccessToken = result.AccessToken
            };
        }
    }
}

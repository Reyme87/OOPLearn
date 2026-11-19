using ApiGateway.Dtos;
using Auth;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthGrpcService.AuthGrpcServiceClient _authClient;

        public AuthController(AuthGrpcService.AuthGrpcServiceClient authClient) => _authClient = authClient;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            try
            {
                var response = await _authClient.RegisterAsync(
                    new RegisterRequest
                    {
                        Email = request.Email,
                        Password = request.Password,
                        FirstName = request.FirstName,
                        LastName = request.LastName
                    });

                return Ok(MapResponse(response));
            }
            catch (RpcException ex)
            {
                return HandleGrpcException(ex);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            try
            {
                var response = await _authClient.LoginAsync(
                    new LoginRequest
                    {
                        Email = request.Email,
                        Password = request.Password
                    });

                return Ok(MapResponse(response));
            }
            catch (RpcException ex)
            {
                return HandleGrpcException(ex);
            }
        }

        private static object MapResponse(AuthResponse response)
        {
            return new
            {
                userId = response.UserId,
                email = response.Email,
                firstName = response.FirstName,
                lastName = response.LastName,
                accessToken = response.AccessToken
            };
        }

        private IActionResult HandleGrpcException(RpcException ex)
        {
            return ex.StatusCode switch
            {
                Grpc.Core.StatusCode.InvalidArgument =>
                    BadRequest(new { message = ex.Status.Detail }),

                Grpc.Core.StatusCode.AlreadyExists =>
                    Conflict(new { message = ex.Status.Detail }),

                Grpc.Core.StatusCode.Unauthenticated =>
                    Unauthorized(new { message = ex.Status.Detail }),

                Grpc.Core.StatusCode.Unavailable =>
                    StatusCode(503, new { message = "Сервис авторизации временно недоступен." }),

                _ =>
                    StatusCode(502, new { message = "Ошибка при обращении к сервису авторизации." })
            };
        }
    }
}

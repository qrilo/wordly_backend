using System.Threading.Tasks;
using Wordly.Application.Models.Auth;

namespace Wordly.Application.Contracts;

public interface IAuthService
{
    Task<UserCreatedResponse> CreateUser(UserRegisterRequest request);
    Task<AuthResponse> GenerateJwtSession(SignInRequest request);
    Task<AuthResponse> RefreshJwtSession(RefreshTokenRequest request);
}
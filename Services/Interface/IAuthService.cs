using TaskFlow.DTOs.Auth;

namespace TaskFlow.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    }
}

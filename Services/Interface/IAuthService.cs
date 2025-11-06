using TaskFlow.DTOs;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterDto registerDto);
    }
}

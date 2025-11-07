using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.DTOs.Auth;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            //  Validate input
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("All fields are required.");
            }

            //  Check email format
            if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(request.Email))
                throw new ArgumentException("Invalid email format.");

            //  Prevent duplicates
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Email already registered.");

            //  Hash password securely
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = "Member",
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            //  Save to DB with exception handling
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New user registered: {Email}", request.Email);

                return new RegisterResponse
                {
                    Message = "User registered successfully",
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                throw new Exception("Registration failed, please try again later.");
            }
        }
    }
}

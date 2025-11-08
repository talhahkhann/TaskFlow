using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.DTOs;
using TaskFlow.Helpers;
using TaskFlow.Models;
using TaskFlow.Repositories;

namespace TaskFlow.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository, 
            IMapper mapper, 
            JwtHelper jwtHelper,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Check for existing user
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            // Map DTO to User entity
            var newUser = _mapper.Map<User>(request);

            // Hash password using IPasswordHasher
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            // Save user
            var savedUser = await _userRepository.AddUserAsync(newUser);

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(savedUser);

            // Prepare response
            var response = _mapper.Map<RegisterResponseDto>(savedUser);
            response.Token = token;
            response.Message = "User registered successfully.";
            return response;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid email or password.");

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Invalid email or password.");

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(user);

            // Prepare response
            var response = _mapper.Map<LoginResponseDto>(user);
            response.Token = token;
            response.Message = "Login successful.";
            return response;
        }
    }
}

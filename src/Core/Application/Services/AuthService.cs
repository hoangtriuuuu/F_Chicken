using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<AuthResponseDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Người dùng không tồn tại" };
            }

            user.FullName = dto.FullName;
            user.Phone = dto.Phone;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                Success = true,
                Message = "Cập nhật thông tin thành công",
                User = MapToDto(user)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if email already exists
            var existingUsers = await _unitOfWork.Users.GetAsync(u => u.Email == dto.Email);
            if (existingUsers.Any())
            {
                return new AuthResponseDto { Success = false, Message = "Email đã được sử dụng" };
            }

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Phone = dto.Phone,
                Role = "Customer",
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đăng ký thành công",
                Token = token,
                User = MapToDto(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var users = await _unitOfWork.Users.GetAsync(u => u.Email == dto.Email);
            var user = users.FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new AuthResponseDto { Success = false, Message = "Email hoặc mật khẩu không đúng" };
            }

            if (!user.IsActive)
            {
                return new AuthResponseDto { Success = false, Message = "Tài khoản đã bị khóa" };
            }

            // Fetch user claims
            var userClaims = await _unitOfWork.UserClaims.GetAsync(uc => uc.UserId == user.Id);
            var claimsList = userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();

            var token = GenerateJwtToken(user, claimsList);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Token = token,
                User = MapToDto(user)
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        private string GenerateJwtToken(User user, List<Claim> additionalClaims = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKey12345!@#$%"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    // Avoid duplicate claims if any
                    if (!claims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                    {
                        claims.Add(claim);
                    }
                }
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "FriedChickenApp",
                audience: _configuration["Jwt:Audience"] ?? "FriedChickenApp",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Role = user.Role
            };
        }
    }
}

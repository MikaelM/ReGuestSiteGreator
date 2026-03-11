using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReGuestSiteGreator.Application.DTOs.Auth;
using ReGuestSiteGreator.Application.Interfaces;
using ReGuestSiteGreator.Infrastructure.Data;

namespace ReGuestSiteGreator.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = GenerateJwtToken(user.Id, user.Email, user.Role.ToString());
        var expiresAt = DateTime.UtcNow.AddHours(GetTokenExpiryHours());

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            Role = user.Role.ToString(),
            UserId = user.Id
        };
    }

    private string GenerateJwtToken(Guid userId, string email, string role)
    {
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is not configured.");
        var jwtIssuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer is not configured.");
        var jwtAudience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(GetTokenExpiryHours()),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetTokenExpiryHours()
    {
        var expiry = _configuration["Jwt:ExpiryHours"];
        return int.TryParse(expiry, out var hours) ? hours : 24;
    }
}

using ReGuestSiteGreator.Application.DTOs.Auth;

namespace ReGuestSiteGreator.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}

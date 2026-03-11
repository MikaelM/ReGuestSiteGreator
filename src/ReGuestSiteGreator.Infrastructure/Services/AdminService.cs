using Microsoft.EntityFrameworkCore;
using ReGuestSiteGreator.Application.DTOs.Admin;
using ReGuestSiteGreator.Application.Interfaces;
using ReGuestSiteGreator.Domain.Entities;
using ReGuestSiteGreator.Domain.Enums;
using ReGuestSiteGreator.Infrastructure.Data;

namespace ReGuestSiteGreator.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PartnerResponse>> GetPartnersAsync()
    {
        var partners = await _context.Partners
            .Include(p => p.User)
            .Include(p => p.Plan)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();

        return partners.Select(MapToResponse);
    }

    public async Task<PartnerResponse> CreatePartnerAsync(CreatePartnerRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            throw new InvalidOperationException($"A user with username '{request.Username}' already exists.");

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException($"A user with email '{request.Email}' already exists.");

        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Partner,
            CreatedAt = now,
            UpdatedAt = now
        };

        var partner = new Partner
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CompanyName = request.CompanyName,
            UserId = user.Id,
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.Users.Add(user);
        _context.Partners.Add(partner);
        await _context.SaveChangesAsync();

        partner.User = user;
        return MapToResponse(partner);
    }

    public async Task<IEnumerable<PlanResponse>> GetPlansAsync()
    {
        var plans = await _context.Plans
            .OrderBy(p => p.Type)
            .ToListAsync();

        return plans.Select(p => new PlanResponse
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type,
            Description = p.Description,
            CreatedAt = p.CreatedAt
        });
    }

    public async Task<PartnerResponse> AssignPlanToPartnerAsync(Guid partnerId, AssignPlanRequest request)
    {
        var partner = await _context.Partners
            .Include(p => p.User)
            .Include(p => p.Plan)
            .FirstOrDefaultAsync(p => p.Id == partnerId)
            ?? throw new KeyNotFoundException($"Partner with ID '{partnerId}' was not found.");

        var plan = await _context.Plans.FindAsync(request.PlanId)
            ?? throw new KeyNotFoundException($"Plan with ID '{request.PlanId}' was not found.");

        partner.PlanId = plan.Id;
        partner.Plan = plan;
        partner.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(partner);
    }

    private static PartnerResponse MapToResponse(Partner partner) => new()
    {
        Id = partner.Id,
        Name = partner.Name,
        CompanyName = partner.CompanyName,
        Email = partner.User.Email,
        UserId = partner.UserId,
        PlanId = partner.PlanId,
        PlanName = partner.Plan?.Name,
        CreatedAt = partner.CreatedAt,
        UpdatedAt = partner.UpdatedAt
    };
}

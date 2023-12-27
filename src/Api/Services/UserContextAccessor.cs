using System.Security.Claims;
using Api.Constants;
using Application.Interfaces;

namespace Api.Services;

public class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsIdentity? Identity => (ClaimsIdentity?) _httpContextAccessor.HttpContext?.User.Identity;

    public Guid? Id
    {
        get
        {
            string? id = Identity?.Claims.FirstOrDefault(c => c.Type == ExtraClaimTypes.UserId)?.Value;

            return !string.IsNullOrEmpty(id) ? new Guid(id) : null;
        }
    }

    public string? Oid => Identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    public string? Email => Identity?.Claims.FirstOrDefault(c => c.Type == ExtraClaimTypes.Email)?.Value;
}
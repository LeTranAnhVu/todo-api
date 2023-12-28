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

    public Guid Id
    {
        get
        {
            string? id = Identity?.Claims.FirstOrDefault(c => c.Type == ExtraClaimTypes.UserId)?.Value;

            return !string.IsNullOrEmpty(id) ? new Guid(id) : throw new NullReferenceException("Cannot get user id");
        }
    }

    public string Oid
    {
        get
        {
            string? oid = Identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(oid) ? oid : throw new NullReferenceException("Cannot get user oid");
        }
    }

    public string Email 
    {
        get
        {
            string? email = Identity?.Claims.FirstOrDefault(c => c.Type == ExtraClaimTypes.Email)?.Value;
            return !string.IsNullOrEmpty(email) ? email : throw new NullReferenceException("Cannot get user email");
        }
    }
}
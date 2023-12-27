using System.Security.Claims;
using Api.Constants;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Api.Filters;

public class ActivateUserAuthorizationFilter(IUserService userService, IServiceProvider serviceProvider) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var identity = (ClaimsIdentity?)context.HttpContext.User.Identity;
        if (identity is not null)
        {
            string oid = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            string email = identity.Claims.FirstOrDefault(c => c.Type == ExtraClaimTypes.Email)?.Value ?? "";
            
            // TODO cache here
            User? user = await userService.FindByOidAsync(oid);
            
            if (user is null)
            {
                // Add user
                var newUser = User.Create(oid, email);
                user = await userService.CreateUserAsync(newUser);
            }
            
            identity.AddClaim(new Claim(ExtraClaimTypes.UserId, user.Id.ToString()));
        }

    }
}
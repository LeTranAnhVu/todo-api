using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController: ControllerBase
{
    private readonly IUserContextAccessor _userContextAccessor;

    public TasksController(IUserContextAccessor userContextAccessor)
    {
        _userContextAccessor = userContextAccessor;
    }
    
    [HttpGet]
    public string Get()
    {
        var a=  _userContextAccessor.Id.ToString() ?? "";
        return a;
    }
}
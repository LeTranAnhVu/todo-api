using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController: ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "sakdfkasdf";
    }
}
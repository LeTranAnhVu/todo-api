using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TodoStatusesController : ControllerBase
{
   private readonly ITodoStatusService _statusService;

   public TodoStatusesController(ITodoStatusService statusService)
   {
      _statusService = statusService;
   }
   
   [HttpPost]
   public async Task<ActionResult<TodoStatusDto>> Create(UpsertTodoStatusDto dto)
   {
      return Ok(await _statusService.CreateTodoStatusAsync(dto));
   }
   
   [HttpPut("{id:guid}")]
   public async Task<ActionResult<TodoStatusDto>> Update(Guid id, UpsertTodoStatusDto dto)
   {
      return Ok(await _statusService.UpdateTodoStatusAsync(id, dto));
   }
}
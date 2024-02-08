using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.DTOs;

public class CreateTodoDto
{
    public required string Name { get; set; }
    public ICollection<UpsertNestedSubTodoDto>? SubTodos { get; set; }
    
    [EnumDataType(typeof(RepeatableType))]
    public RepeatableType? RepeatableType { get; set; } 
    
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
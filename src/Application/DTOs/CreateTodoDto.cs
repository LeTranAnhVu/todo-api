using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.DTOs;

public class CreateNestedSubTodoDto
{
    public required string Name { get; set; }
    public ICollection<CreateTodoDto>? SubTodos { get; set; }
    
    [EnumDataType(typeof(RepeatableType))]
    public RepeatableType? RepeatableType { get; set; } 
}

public class CreateTodoDto
{
    public required string Name { get; set; }
    public ICollection<CreateNestedSubTodoDto>? SubTodos { get; set; }
    
    [EnumDataType(typeof(RepeatableType))]
    public RepeatableType? RepeatableType { get; set; } 
    
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
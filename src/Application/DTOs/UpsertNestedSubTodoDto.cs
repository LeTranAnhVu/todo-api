using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Application.DTOs;

public class UpsertNestedSubTodoDto
{
    public required string Name { get; set; }
    public Guid? Id { get; set; }
    
    [EnumDataType(typeof(RepeatableType))]
    public RepeatableType? RepeatableType { get; set; } 
    
}
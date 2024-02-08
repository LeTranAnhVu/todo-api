namespace Application.DTOs;

public class UpdateTodoDto
{
    public required string Name { get; set; }
    public ICollection<UpsertNestedSubTodoDto>? SubTodos { get; set; }
}
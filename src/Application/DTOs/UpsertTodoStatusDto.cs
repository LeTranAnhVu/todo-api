﻿namespace Application.DTOs;

public class UpsertTodoStatusDto
{
    public Guid TodoId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? OccuredAt { get; set; }
}
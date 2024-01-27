﻿using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Todo, SubTodoDto>()
            .ForMember(
                dto => dto.RepeatableType,
                opt => opt.MapFrom(todo => todo.Repeatable.Type))
            .ForMember(
                dto => dto.StartedAt, opt =>
                    opt.MapFrom(todo => todo.Repeatable.StartedAt))
            .ForMember(
            dto => dto.EndedAt, opt =>
                opt.MapFrom(todo => todo.Repeatable.EndedAt));

        CreateMap<Todo, TodoDto>()
            .ForMember(
                dto => dto.RepeatableType,
                opt => opt.MapFrom(todo => todo.Repeatable.Type))
            .ForMember(
                dto => dto.StartedAt,
                opt => opt.MapFrom(todo => todo.Repeatable.StartedAt))
            .ForMember(
                dto => dto.EndedAt,
                opt => opt.MapFrom(todo => todo.Repeatable.EndedAt));

        CreateMap<TodoStatus, TodoStatusDto>()
            .ForMember(dto => dto.TodoName,
                otp => otp.MapFrom(stt => stt.Todo.Name));
        // src -> dest
    }
}
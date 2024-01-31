using Application.DTOs;
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
                dto => dto.StartDate, opt =>
                    opt.MapFrom(todo => todo.Repeatable.StartDate))
            .ForMember(
            dto => dto.EndDate, opt =>
                opt.MapFrom(todo => todo.Repeatable.EndDate));

        CreateMap<Todo, TodoDto>()
            .ForMember(
                dto => dto.RepeatableType,
                opt => opt.MapFrom(todo => todo.Repeatable.Type))
            .ForMember(
                dto => dto.StartDate,
                opt => opt.MapFrom(todo => todo.Repeatable.StartDate))
            .ForMember(
                dto => dto.EndDate,
                opt => opt.MapFrom(todo => todo.Repeatable.EndDate));

        CreateMap<TodoStatus, TodoStatusDto>()
            .ForMember(dto => dto.TodoName,
                otp => otp.MapFrom(stt => stt.Todo.Name));
        // src -> dest
    }
}
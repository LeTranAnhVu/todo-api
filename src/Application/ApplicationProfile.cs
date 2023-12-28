using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Todo, TodoDto>()
            .ForMember(dto => dto.RepeatableType, opt => opt.MapFrom(todo => todo.Repeatable!.Type))
            .ForMember(dto => dto.RepeatableStartedAt, opt => opt.MapFrom(todo => todo.Repeatable!.StartedAt));
        // CreateMap<Repeatable, RepeatableDto>();
        // src -> dest


    }
}
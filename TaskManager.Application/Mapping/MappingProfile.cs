using AutoMapper;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //TaskItem
        CreateMap<TaskItem, TaskDto>().
            ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels.Select(l => l.Name)));
        CreateMap<CreateTaskDto, TaskItem>().
            ForMember(dest => dest.Labels, opt => opt.Ignore());

        //Label
        CreateMap<Label, LabelDto>();
        CreateMap<CreateLabelDto, Label>();
    }
}
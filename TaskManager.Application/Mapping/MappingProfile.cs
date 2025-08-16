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
        // Domain -> DTO
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.Labels,
                opt => opt.MapFrom(src => src.TaskLabels.Select(tl => tl.Label.Name)));

        CreateMap<Label, string>().ConvertUsing(l => l.Name);

        // DTO -> Domain (Create)
        CreateMap<CreateTaskDto, TaskItem>()
            .ForMember(dest => dest.TaskLabels, opt => opt.Ignore());

        // DTO -> Domain (Update)
        CreateMap<UpdateTaskDto, TaskItem>()
            .ForMember(dest => dest.TaskLabels, opt => opt.Ignore());

        //Label
        CreateMap<Label, LabelDto>();
        CreateMap<CreateLabelDto, Label>();
    }
}
﻿namespace TaskManager.Application.DTOs.Task;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public List<string>? Labels { get; set; }
}

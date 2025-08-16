using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.DTOs.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public PaginationMetadata? Metadata { get; set; }

    public static ApiResponse<T> ReturnSuccess(T data, PaginationMetadata? metadata = null)
        => new() { Success = true, Data = data, Metadata = metadata };

    public static ApiResponse<T> ReturnError(string error)
        => new() { Success = false, Error = error };
}
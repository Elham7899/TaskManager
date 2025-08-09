namespace TaskManager.API.Responses;

public record ApiResponse<T>(bool Success, T? Data, string? Error, PaginationMetadata? Pagination = null)
{
    public  static ApiResponse<T> ReturnSuccess(T data, PaginationMetadata? pagination = null) =>
        new(true, data, null, pagination);

    public static ApiResponse<T> Fail(string error) =>
        new(false, default, error);
}

public record PaginationMetadata(int Page, int PageSize, int TotalCount);

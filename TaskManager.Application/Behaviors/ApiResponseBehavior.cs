using MediatR;
using TaskManager.Application.DTOs.Common;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Behaviors;

public class ApiResponseBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ApiResponse<TResponse>>
    where TRequest : IRequest<ApiResponse<TResponse>>
{
    private readonly ILogger<ApiResponseBehavior<TRequest, TResponse>> _logger;

    public ApiResponseBehavior(ILogger<ApiResponseBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<ApiResponse<TResponse>> Handle(
        TRequest request,
        RequestHandlerDelegate<ApiResponse<TResponse>> next,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();
            return response;
        }
        catch (ValidationException ex)
        {
            return ApiResponse<TResponse>.ReturnError($"Validation failed: {ex.Message}");
        }
        catch (KeyNotFoundException ex)
        {
            return ApiResponse<TResponse>.ReturnError($"Not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in {Request}", typeof(TRequest).Name);
            return ApiResponse<TResponse>.ReturnError("An unexpected error occurred.");
        }
    }
}


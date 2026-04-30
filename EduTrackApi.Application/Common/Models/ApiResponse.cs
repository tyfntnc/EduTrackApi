namespace EduTrackApi.Application.Common.Models;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; } = true;
    public T? Data { get; init; }
    public MetaData? Meta { get; init; }
    public ApiError? Error { get; init; }

    public static ApiResponse<T> Ok(T data, MetaData? meta = null) => new() { Success = true, Data = data, Meta = meta };
    public static ApiResponse<T> Fail(string code, string message) => new() { Success = false, Error = new ApiError { Code = code, Message = message } };
}

public sealed class ApiResponse
{
    public bool Success { get; init; } = true;
    public object? Data { get; init; }
    public MetaData? Meta { get; init; }
    public ApiError? Error { get; init; }

    public static ApiResponse Ok(object? data = null, MetaData? meta = null) => new() { Success = true, Data = data, Meta = meta };
    public static ApiResponse Fail(string code, string message) => new() { Success = false, Error = new ApiError { Code = code, Message = message } };
}

public sealed class MetaData
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
    public int? UnreadCount { get; init; }
}

public sealed class ApiError
{
    public string Code { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string? Details { get; init; }
}

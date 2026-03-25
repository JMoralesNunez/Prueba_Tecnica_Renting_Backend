namespace Eventia.Application.Common;

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static BaseResponse<T> Failure(string message, List<string>? errors = null) => 
        new() { Success = false, Message = message, Errors = errors };
    
    public static BaseResponse<T> SuccessResult(T data, string message = "Success") => 
        new() { Success = true, Message = message, Data = data };
}

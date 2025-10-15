namespace Academy.Documents.Domain.Shared;

public class Result
{
    public string? Status { get; set; }

    public string? Message { get; set; }

    public bool IsSuccess { get; set; }

    public Error? Error { get; set; }

    public int? StatusCode { get; set; }

    public static Result Success() => new() { IsSuccess = true };

    public static Result Failure(string errorCode, string errorMessage) => new() { IsSuccess = false, Error = new Error(errorCode, errorMessage) };
    public static Result Failure(int statusCode, string errorCode, string errorMessage) => new() { IsSuccess = false, StatusCode = statusCode, Error = new Error(errorCode, errorMessage) };

}
public class Result<TResponse> : Result
{
    public TResponse? Value { get; set; }

    public static Result<TResponse> Success(TResponse value) => new() { IsSuccess = true, Status = "S", Value = value };
    public static Result<TResponse> Success(TResponse value, string message) => new() { IsSuccess = true, Status = "S", Message = message, Value = value };
    public static Result<TResponse> Failure(Error error) => new() { IsSuccess = false, Status = "E", Error = error };
    public static Result<TResponse> Failure(string error) => new() { IsSuccess = false, Status = "E", Message = error };
    public static Result<TResponse> Failure(int statusCode, string errorCode, string errorMessage) => new() { IsSuccess = false, StatusCode = statusCode, Error = new Error(errorCode, errorMessage) };
}

public sealed record Error(string ErrorCode, string ErrorMessage);
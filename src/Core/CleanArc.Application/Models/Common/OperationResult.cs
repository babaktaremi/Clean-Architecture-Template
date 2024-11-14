namespace CleanArc.Application.Models.Common;

/// <summary>
/// Marker Interface To Mark Operation Result Response In Validation Pipeline
/// </summary>
public interface IOperationResult
{
    bool IsSuccess { get; set; }
    List<KeyValuePair<string,string>> ErrorMessages { get; set; }
    bool IsException { get; set; }
    bool IsNotFound { get; set; }
}

public class OperationResult<TResult> : IOperationResult
{
    public TResult Result { get;  set; }

    public bool IsSuccess { get; set; }
    public List<KeyValuePair<string,string>> ErrorMessages { get; set; } = new();
    public bool IsException { get; set; }
    public bool IsNotFound { get; set; }

    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult> { Result = result, IsSuccess = true };
    }

    public static OperationResult<TResult> FailureResult(string propertyName, string message, TResult result = default)
    {
        var operationResult = new OperationResult<TResult> { Result = result, IsSuccess = false };

        operationResult.ErrorMessages.Add(new(propertyName,message));

        return operationResult;
    }    
    public static OperationResult<TResult> FailureResult(string message, TResult result = default)
    {
        var operationResult = new OperationResult<TResult> { Result = result, IsSuccess = false };

        operationResult.ErrorMessages.Add(new("GeneralError",message));

        return operationResult;
    }
    public static OperationResult<TResult> NotFoundResult(string message)
    {
        var operationResult = new OperationResult<TResult> { IsSuccess = false, IsNotFound = true };

        operationResult.ErrorMessages.Add(new("GeneralError", message));

        return operationResult;
    }

    public void AddError(string propertyName, string message)
    {
        IsSuccess = false;
        
        ErrorMessages.Add(new(propertyName,message));
    }
    
    public string GetErrorMessage()
    => string.Join(Environment.NewLine, ErrorMessages.Select(x => $"{x.Key}: {x.Value}"));
}
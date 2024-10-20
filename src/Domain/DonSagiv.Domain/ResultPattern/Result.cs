using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DonSagiv.Domain.ResultPattern;

public class Result : IResult
{
    #region Properties
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IError Error { get; }
    #endregion

    #region Constructor
    protected internal Result(bool isSuccess, IError? error = null)
    {
        Error = error ?? ResultPattern.Error.None;

        this.IsSuccess = isSuccess;

        if (isSuccess && error is not null)
        {
            throw new ArgumentException("No error value can be passed if result is successful.");
        }

        if (IsFailure && error is null)
        {
            throw new ArgumentException("Error result must have an error code and description.");
        }
    }
    #endregion

    #region Static Methods
    public static IResult Success()
    {
        return new Result(true);
    }

    public static IResult Failure(IError errorInput)
    {
        return new Result(false, errorInput);
    }

    public static IResult Failure(string errorDescription)
    {
        return Failure(ResultPattern.Error.FromDescription(errorDescription));
    }

    public static IResult Failure(Exception exception)
    {
        return new Result(false, ResultPattern.Error.FromException(exception));
    }

    public static IResult<TValue> Success<TValue>(TValue valueInput)
    {
        return new Result<TValue>(valueInput, true);
    }

    public static IResult<TValue> Failure<TValue>(IError error, TValue? valueInput = default)
    {
        return new Result<TValue>(valueInput, false, error);
    }

    public static IResult<TValue> Failure<TValue>(Exception exception, TValue? valueInput = default)
    {
        return new Result<TValue>(valueInput, false, ResultPattern.Error.FromException(exception));
    }

    public static IResult<TValue> FromResult<TValue>(IResult sourceResult, TValue? valueInput = default)
    {
        return new Result<TValue>(valueInput, sourceResult.IsSuccess, sourceResult.Error);
    }

    public override string ToString()
    {
        if (IsSuccess)
        {
            return "Result is successful.";
        }

        var sb = new StringBuilder();

        sb.AppendLine($"Result failed with code {Error.Code}: {Error.Description}");

        foreach(var line in Error.DescriptionLines)
        {
            sb.AppendLine(line);
        }

        return sb.ToString();
    }

    #endregion
}

public sealed class Result<TValue> : Result, IResult<TValue>
{
    #region Properties
    public TValue? Value { get; }
    #endregion

    #region Constructor
    internal Result(TValue? valueInput, bool isSuccess, IError? error = null) : base(isSuccess, error)
    {
        this.Value = valueInput;
    }
    #endregion
}

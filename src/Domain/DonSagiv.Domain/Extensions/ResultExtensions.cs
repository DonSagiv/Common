using DonSagiv.Domain.ResultPattern;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DonSagiv.Domain.Extensions;

public static class ResultExtensions
{
    #region Static Methods
    public static IResult ToResult(this Exception exception)
    {
        return new Result(false, Error.FromException(exception));
    }

    public static IResult<TValue> ToResult<TValue>(this Exception exception, TValue? valueInput = default)
    {
        return new Result<TValue>(valueInput, false, Error.FromException(exception));
    }

    public static IResult ToResult(this IResult sourceResult)
    {
        return new Result(sourceResult.IsSuccess, sourceResult.Error);
    }

    public static IResult ToResult<TValue>(this IResult resultInput, TValue? valueInput = default)
    {
        return new Result<TValue>(valueInput, resultInput.IsSuccess, resultInput.Error);
    }

    public static Task<IResult> Async(this IResult sourceResult)
    {
        return Task.FromResult(sourceResult);
    }

    public static Task<IResult<TValue>> Async<TValue>(this IResult<TValue> sourceResult)
    {
        return Task.FromResult(sourceResult);
    }

    private static Error GetResultError(this IResult resultInput)
    {
        if (resultInput.IsSuccess || resultInput.Error is not Error error)
        {
            throw new ArgumentException("Error cannot be retrieved from a successful result.");
        }

        return error;
    }

    public static IResult WithCode(this IResult resultInput, string errorCode)
    {
        var error = resultInput.GetResultError();

        error.Code = errorCode;

        return resultInput;
    }

    public static IResult AddLine(this IResult resultInput, string lineToAdd)
    {
        var error = resultInput.GetResultError();

        error.DescriptionLines = [.. error.DescriptionLines, lineToAdd];

        return resultInput;
    }

    public static IObservable<IResult<TValue>> ToResult<TValue>(this ISubject<TValue> subjectInput)
    {
        return subjectInput.Select(Result.Success);
    }
    #endregion
}
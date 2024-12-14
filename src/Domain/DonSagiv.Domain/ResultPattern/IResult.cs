namespace DonSagiv.Domain.ResultPattern;

public interface IResult
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public IError Error { get; }
}

public interface IResult<TValue> : IResult
{
    TValue? Value { get; }
}

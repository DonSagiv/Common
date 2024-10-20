namespace DonSagiv.Domain.ResultPattern;

public interface IError
{
    Exception? Exception { get; }
    string Code { get; }
    string Description { get; }
    string[] DescriptionLines { get; }
}

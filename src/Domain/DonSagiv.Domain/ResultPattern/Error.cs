namespace DonSagiv.Domain.ResultPattern;

public class Error : IError
{
    #region Statics
    public static Error None => new(string.Empty, "No error has occurred.");
    #endregion

    #region Properties
    public Exception? Exception { get; }
    public string Code { get; internal set; }
    public string Description { get; }
    public string[] DescriptionLines { get; internal set; }
    #endregion

    #region Constructor
    private Error(string code,
        string description,
        Exception? exception = null,
        params string[] descriptionLines)
    {
        Code = code;
        Description = description;
        Exception = exception;
        DescriptionLines = descriptionLines;
    }
    #endregion

    #region Methods
    public static Error FromDescription(string description, 
        string code = ErrorCode.Undefined, 
        params string[] lines)
    {
        return new Error(code, description, descriptionLines: lines);
    }

    public static Error FromException(Exception exception,
        string code = ErrorCode.Undefined)
    {
        return new Error(code, exception.Message, exception);
    }
    #endregion
}

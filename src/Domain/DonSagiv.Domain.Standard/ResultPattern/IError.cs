using System;

namespace DonSagiv.Domain.Standard.ResultPattern;

public interface IError
{
    Exception? Exception { get; }
    string Code { get; }
    string Description { get; }
    string[] DescriptionLines { get; }
}

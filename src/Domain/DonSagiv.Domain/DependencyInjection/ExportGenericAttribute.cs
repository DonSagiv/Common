namespace DonSagiv.Domain.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExportGenericAttribute : ExportAttribute
{
    #region Constructor
    public ExportGenericAttribute(Type contractType, object? contractKey = null) : base(contractType, contractKey)
    {
        if(!contractType.IsGenericType)
        {
            throw new ArgumentException("Cannot use GenericExport with non-generic types. Use Export instead.");
        }
    }
    #endregion
}

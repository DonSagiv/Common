namespace DonSagiv.Domain.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExportAttribute(Type contractType,
    object? contractKey = null,
    CreationPolicy creationPolicy = CreationPolicy.Transient) : Attribute
{
    #region Properties
    public Type ContractType { get; } = contractType;
    public object? ContractKey { get; } = contractKey;
    public CreationPolicy CreationPolicy { get; } = creationPolicy;
    #endregion
}

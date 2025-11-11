namespace DonSagiv.Domain.Entities;

public abstract class EntityModelBase : IEntityModel
{
    public Ulid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Version? CreatedVersion { get; set; }
    public DateTime LastSavedDate { get; set; }
    public Version? LastVersion { get; set; }
}

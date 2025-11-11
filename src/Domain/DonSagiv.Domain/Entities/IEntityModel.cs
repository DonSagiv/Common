namespace DonSagiv.Domain.Entities;

public interface IEntityModel : IEntityVersion
{
    Ulid Id { get; set; }
}

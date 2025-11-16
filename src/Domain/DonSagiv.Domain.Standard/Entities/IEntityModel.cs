using System;

namespace DonSagiv.Domain.Standard.Entities;

public interface IEntityModel : IEntityVersion
{
    Ulid Id { get; set; }
}

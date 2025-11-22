using System;

namespace DonSagiv.Domain.Standard.Entities;

public abstract class EntityModelBase : IEntityModel
{
    public Ulid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Version CreatedVersion { get; set; }
    public DateTime LastSavedDate { get; set; }
    public Version LastVersion { get; set; }
}

public abstract class EntityModel<TModel> : EntityModelBase, IEntityModel<TModel>
{
    public TModel Model { get; set; }
}

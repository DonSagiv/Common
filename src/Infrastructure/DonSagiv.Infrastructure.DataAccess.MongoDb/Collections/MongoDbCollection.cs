using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.Entities;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;

public abstract class MongoDbCollection : IMongoDbCollection
{
    public required   string SetName { get; set; }
}

[Export(typeof(IMongoDbCollection))]
internal class MongoDbCollection<TModel> : MongoDbCollection, IMongoDbCollection<TModel>
    where TModel : IEntityModel
{
    #region Fields
    private IMongoCollection<TModel>? _collection;
    #endregion

    #region Methods
    internal void SetCollection(IMongoCollection<TModel> collectionInput)
    {
        _collection = collectionInput;
    }
    #endregion
}

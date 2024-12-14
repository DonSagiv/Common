using DonSagiv.Domain.DependencyInjection;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Interfaces;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Models;

public abstract class MongoDbCollection : IMongoDbCollection
{
    public string collectionName { get; set; }
}

[Export(typeof(IMongoDbCollection))]
internal class MongoDbCollection<TModel> : MongoDbCollection, IMongoDbCollection<TModel>
{
    #region Fields
    private IMongoCollection<TModel> _collection;
    #endregion

    #region Methods
    internal void SetCollection(IMongoCollection<TModel> collectionInput)
    {
        _collection = collectionInput;
    }
    #endregion
}

using DonSagiv.Domain.DependencyInjection;
using DonSagiv.Domain.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Interfaces;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Models;

[Export(typeof(IMongoDbDatabase))]
internal class MongoDbDatabase(Func<IMongoDbCollection> collectionFactory) : IMongoDbDatabase
{
    #region Fields
    private readonly Func<IMongoDbCollection> _collectionFactory = collectionFactory;

    private IMongoDatabase? _mongoDatabase;
    #endregion

    #region Methods
    internal void SetMongoDatabase(IMongoDatabase? mongoDatabaseInput)
    {
        _mongoDatabase = mongoDatabaseInput;
    }

    public IResult<IMongoDbCollection<TModel>> GetCollection<TModel>(string collectionName)
    {
        if(_mongoDatabase is null)
        {
            return Result.Failure<IMongoDbCollection<TModel>>(Error.FromDescription("No Mongo Database could be found."));
        }

        var mongoCollection = _mongoDatabase.GetCollection<TModel>(collectionName, new MongoCollectionSettings());

        var collection = _collectionFactory();

        if(collection is not MongoDbCollection<TModel> collectionImpl)
        {
            return Result.Failure<IMongoDbCollection<TModel>>(Error.FromDescription("Incorrect collection type."));
        }

        collectionImpl.SetCollection(mongoCollection);

        return Result.Success(collection);
    }
    #endregion
}

using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;

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

    public IResult<IMongoDbCollection<TModel>> GetCollection<TModel>(string? collectionName)
    {
        if (_mongoDatabase is null)
        {
            return Result.Failure<IMongoDbCollection<TModel>>(Error.FromDescription("No Mongo Database could be found."));
        }

        var mongoCollection = _mongoDatabase.GetCollection<TModel>(collectionName, new MongoCollectionSettings());

        var collection = _collectionFactory();

        if (collection is not MongoDbCollection<TModel> collectionImpl)
        {
            return Result.Failure<IMongoDbCollection<TModel>>(Error.FromDescription("Incorrect collection type."));
        }

        collectionImpl.SetCollection(mongoCollection);

        return Result.Success(collectionImpl as IMongoDbCollection<TModel>);
    }
    #endregion
}

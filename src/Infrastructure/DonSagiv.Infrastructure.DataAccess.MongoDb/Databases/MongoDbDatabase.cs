using DonSagiv.Appl.DataAccess.RepositorySets;
using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.Entities;
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

    #region Properties
    public required string RepositoryName { get; set; }
    #endregion

    #region Methods
    internal void SetMongoDatabase(IMongoDatabase? mongoDatabaseInput)
    {
        _mongoDatabase = mongoDatabaseInput;
    }

    public IResult<IMongoDbCollection<TEntityModel>> GetCollection<TEntityModel>(string? collectionName)
        where TEntityModel : IEntityModel
    {
        if (_mongoDatabase is null)
        {
            return Result.Failure<IMongoDbCollection<TEntityModel>>(Error.FromDescription("No Mongo Database could be found."));
        }

        var mongoCollection = _mongoDatabase.GetCollection<TEntityModel>(collectionName, new MongoCollectionSettings());

        var collection = _collectionFactory();

        if (collection is not MongoDbCollection<TEntityModel> collectionImpl)
        {
            return Result.Failure<IMongoDbCollection<TEntityModel>>(Error.FromDescription("Incorrect collection type."));
        }

        collectionImpl.SetCollection(mongoCollection);

        return Result.Success(collectionImpl as IMongoDbCollection<TEntityModel>);
    }

    public IResult<IRepositorySet<TEntityModel>> GetRepositorySet<TEntityModel>(string setName) where TEntityModel : IEntityModel
    {
        throw new NotImplementedException();
    }
    #endregion
}

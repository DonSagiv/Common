using DonSagiv.Domain.DependencyInjection;
using DonSagiv.Domain.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Interfaces;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Models;

[Export(typeof(IMongoDbHost))]
internal class MongoDbHost(Func<IMongoDbDatabase> databaseFactory) : IMongoDbHost
{
    #region Fields
    private readonly Func<IMongoDbDatabase> databaseFactory = databaseFactory;

    private string? _connectionString;
    private IMongoClient? _mongoClient;
    #endregion

    #region Properties
    public string? ConnectionString 
    {
        get => _connectionString;
        set
        {
            _connectionString = value;
            OnConnectionStringChanged(value);
        }
    }
    #endregion

    #region Methods
    public IResult<IMongoDbDatabase> GetDatabase(string databaseName)
    {
        var mongoDatabase = _mongoClient?.GetDatabase(databaseName);

        var database = databaseFactory();

        if(database is not MongoDbDatabase databaseImpl)
        {
            return Result.Failure(Error.FromDescription("Invalid MongoDB Database type."), database);
        }

        databaseImpl.SetMongoDatabase(mongoDatabase);

        return Result.Success(database);
    }

    private void OnConnectionStringChanged(string? connectionStringInput)
    {
        if (string.IsNullOrWhiteSpace(connectionStringInput))
        {
            _mongoClient = null;

            return;
        }

        _mongoClient = new MongoClient(connectionStringInput);
    }
    #endregion
}

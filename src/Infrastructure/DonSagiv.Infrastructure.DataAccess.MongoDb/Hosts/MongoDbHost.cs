using DonSagiv.Appl.DataAccess.Repositories;
using DonSagiv.Appl.DataAccess.RepositoryHosts;
using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.Extensions;
using DonSagiv.Domain.Standard.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Hosts;

[Export(typeof(IRepositoryHost), contractKey: "MongoDb")]
[Export(typeof(IMongoDbHost))]
internal class MongoDbHost(Func<IMongoDbDatabase> databaseFactory) : IMongoDbHost
{
    #region Fields
    private readonly Func<IMongoDbDatabase> databaseFactory = databaseFactory;

    private string? _connectionString;
    private MongoClient? _mongoClient;
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
    public async Task<IResult> ConnectAsync()
    {
        _mongoClient ??= new MongoClient(ConnectionString);
        
        try
        {
            var adminDb = _mongoClient.GetDatabase("admin");

            var response = await adminDb
                .RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}")
                .WaitAsync(TimeSpan.FromSeconds(5));

            return Result.Success();
        }
        catch(TimeoutException tex)
        {
            return Result.Failure($"MongoDB connection timed out.");
        }
        catch(Exception ex) 
        {
            return ex.ToResult();
        }
    }

    public IResult<IRepository> GetRepository(string repositoryName)
    {
        var database = GetDatabase(repositoryName);

        if (database is not IRepository repository)
        {
            return Result.Failure<IRepository>("Failed to get MongoDB Repository.");
        }

        return Result.Success(repository);
    }


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

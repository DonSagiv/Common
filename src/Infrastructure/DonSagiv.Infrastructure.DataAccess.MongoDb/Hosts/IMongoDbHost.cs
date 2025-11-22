using DonSagiv.Domain.Standard.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Hosts;

public interface IMongoDbHost : IRepositoryHost
{
    IResult<IMongoDbDatabase> GetDatabase(string databaseName);
}

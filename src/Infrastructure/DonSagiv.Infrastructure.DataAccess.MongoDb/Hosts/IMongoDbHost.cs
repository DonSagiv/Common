using DonSagiv.Domain.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Hosts;

internal interface IMongoDbHost
{
    IResult<IMongoDbDatabase> GetDatabase(string databaseName);
}

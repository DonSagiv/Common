using DonSagiv.Domain.ResultPattern;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Interfaces;

internal interface IMongoDbHost
{
    IResult<IMongoDbDatabase> GetDatabase(string databaseName);
}

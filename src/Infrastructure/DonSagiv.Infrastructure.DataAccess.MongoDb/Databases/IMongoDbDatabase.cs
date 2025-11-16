using DonSagiv.Domain.Standard.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;

public interface IMongoDbDatabase
{
    IResult<IMongoDbCollection<TModel>> GetCollection<TModel>(string? collectionName);
}

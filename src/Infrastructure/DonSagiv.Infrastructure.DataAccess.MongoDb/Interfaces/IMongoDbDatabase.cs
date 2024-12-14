using DonSagiv.Domain.ResultPattern;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Interfaces;

public interface IMongoDbDatabase
{
    IResult<IMongoDbCollection<TModel>> GetCollection<TModel>(string? collectionName);
}

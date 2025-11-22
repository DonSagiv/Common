using DonSagiv.Appl.Standard.DataAccess;
using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;
using DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Databases;

public interface IMongoDbDatabase : IRepository
{
    IResult<IMongoDbCollection<TEntityModel>> GetCollection<TEntityModel>(string? collectionName)
        where TEntityModel : IEntityModel;
}

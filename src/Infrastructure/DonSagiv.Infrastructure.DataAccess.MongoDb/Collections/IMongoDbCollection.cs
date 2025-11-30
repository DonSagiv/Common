using DonSagiv.Appl.DataAccess.RepositorySets;
using DonSagiv.Domain.Standard.Entities;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;

public interface IMongoDbCollection : IRepositorySet;

public interface IMongoDbCollection<TEntityModel> : IMongoDbCollection, IRepositorySet<TEntityModel>
    where TEntityModel : IEntityModel;

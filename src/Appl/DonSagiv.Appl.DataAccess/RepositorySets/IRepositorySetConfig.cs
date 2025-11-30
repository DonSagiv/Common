using DonSagiv.Appl.DataAccess.Repositories;
using DonSagiv.Domain.Standard.Entities;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

public interface IRepositorySetConfig
{
    public IRepositoryConfig RepositoryConfig { get; set; }
    public string SetName { get; set; }
}

public interface IRepositorySetConfig<TEntityModel> : IRepositorySetConfig
    where TEntityModel : IEntityModel;

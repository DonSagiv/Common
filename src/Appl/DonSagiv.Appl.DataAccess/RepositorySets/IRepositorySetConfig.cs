using DonSagiv.Appl.DataAccess.Repositories;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

internal interface IRepositorySetConfig
{
    public IRepositoryConfig RepositoryConfig { get; set; }
    public string SetName { get; set; }
}

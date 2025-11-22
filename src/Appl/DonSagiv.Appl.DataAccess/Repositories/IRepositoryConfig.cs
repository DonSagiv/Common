using DonSagiv.Appl.DataAccess.RepositoryHosts;

namespace DonSagiv.Appl.DataAccess.Repositories;

public interface IRepositoryConfig
{
    public IRepositoryHostConfig HostConfig { get; set; }
    public string RepositoryName { get; set; }
}

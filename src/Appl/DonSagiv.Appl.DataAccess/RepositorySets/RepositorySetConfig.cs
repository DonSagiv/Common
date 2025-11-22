using DonSagiv.Appl.DataAccess.Repositories;
using DonSagiv.Domain.Standard.DependencyInjection;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

[Export(typeof(IRepositorySetConfig))]
internal class RepositorySetConfig: IRepositorySetConfig
{
    #region Properties
    public IRepositoryConfig RepositoryConfig { get; set; }
    public string SetName { get; set; }
    #endregion
}

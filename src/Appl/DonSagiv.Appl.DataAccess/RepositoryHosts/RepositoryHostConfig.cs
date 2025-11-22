using DonSagiv.Domain.Standard.DependencyInjection;

namespace DonSagiv.Appl.DataAccess.RepositoryHosts;

[Export(typeof(IRepositoryHostConfig))]
internal class RepositoryHostConfig : IRepositoryHostConfig
{
    #region Properties
    public string DatabasePlatform { get; set; }
    public string ConnectionString { get; set; }
    #endregion
}

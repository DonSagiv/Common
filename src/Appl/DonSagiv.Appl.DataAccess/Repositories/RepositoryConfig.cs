using DonSagiv.Appl.DataAccess.RepositoryHosts;
using DonSagiv.Domain.Standard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonSagiv.Appl.DataAccess.Repositories;

[Export(typeof(IRepositoryConfig))]
internal class RepositoryConfig : IRepositoryConfig
{
    #region Properties
    public IRepositoryHostConfig HostConfig { get; set; }
    public string RepositoryName { get; set; }
    #endregion
}

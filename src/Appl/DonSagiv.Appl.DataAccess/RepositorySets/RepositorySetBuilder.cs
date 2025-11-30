using Autofac.Features.Indexed;
using DonSagiv.Appl.DataAccess.RepositoryHosts;
using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.Extensions;
using DonSagiv.Domain.Standard.ResultPattern;
using System.Threading.Tasks;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

[Export(typeof(IRepositorySetBuilder), creationPolicy: CreationPolicy.Scoped)]
internal class RepositorySetBuilder
{
    #region Fields
    private readonly IIndex<string, IRepositoryHost> repositoryHostFactory;
    #endregion

    #region Constructor
    public RepositorySetBuilder(IIndex<string, IRepositoryHost> repositoryHostFactory)
    {
        this.repositoryHostFactory = repositoryHostFactory;
    }
    #endregion

    #region Methods
    public async Task<IResult<IRepositorySet<TEntityModel>>> BuildAsync<TEntityModel>(IRepositorySetConfig<TEntityModel> config)
        where TEntityModel : IEntityModel
    {
        var host = config.RepositoryConfig.HostConfig.DatabasePlatform;

        if(!repositoryHostFactory.TryGetValue(host, out var repositoryHost))
        {
            return Result.Failure<IRepositorySet<TEntityModel>>($"No repository host found for database platform: {host}");
        }

        var connectResult = await repositoryHost.ConnectAsync();

        if (connectResult.IsFailure)
        {
            return connectResult.ToResult<IRepositorySet<TEntityModel>>();
        }

        var repositoryResult = repositoryHost.GetRepository(config.RepositoryConfig.RepositoryName);

        if(repositoryResult.IsFailure)
        {
            return repositoryResult.ToResult<IRepositorySet<TEntityModel>>();
        }

        repositoryResult.Value.RepositoryName = config.RepositoryConfig.RepositoryName;

        return repositoryResult.Value.GetRepositorySet<TEntityModel>(config.SetName);
    }
    #endregion
}

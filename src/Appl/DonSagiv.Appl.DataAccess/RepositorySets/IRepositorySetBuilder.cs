using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;
using System.Threading.Tasks;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

public interface IRepositorySetBuilder
{
    Task<IResult<IRepositorySet<TEntityModel>>> BuildAsync<TEntityModel>(IRepositorySetConfig<TEntityModel> config)
        where TEntityModel : IEntityModel;
}

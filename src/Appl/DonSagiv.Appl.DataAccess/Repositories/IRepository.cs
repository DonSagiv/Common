using DonSagiv.Appl.DataAccess.RepositorySets;
using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;

namespace DonSagiv.Appl.DataAccess.Repositories;

public interface IRepository
{
    public string RepositoryName { get; set; }

    IResult<IRepositorySet<TEntityModel>> GetRepositorySet<TEntityModel>(string setName)
        where TEntityModel : IEntityModel;
}

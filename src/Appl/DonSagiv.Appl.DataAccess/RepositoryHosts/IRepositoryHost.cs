using DonSagiv.Appl.DataAccess.Repositories;
using DonSagiv.Domain.Standard.ResultPattern;
using System.Threading.Tasks;

namespace DonSagiv.Appl.DataAccess.RepositoryHosts;

public interface IRepositoryHost
{
    Task<IResult> ConnectAsync();
    public IResult<IRepository> GetRepository(string repositoryName);
}

using DonSagiv.Appl.Standard.DataAccess;
using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace DonSagiv.Infrastructure.DataAccess.EntityFramework.DbContexts;

internal class DbContextBase : DbContext, IDbContextBase
{
    public IResult<IRepositorySet<TEntityModel>> GetRepositorySet<TEntityModel>(string setName)
        where TEntityModel : class, IEntityModel
    {
        DbSet< set = Set<TEntityModel>();


    }
    #endregion
}

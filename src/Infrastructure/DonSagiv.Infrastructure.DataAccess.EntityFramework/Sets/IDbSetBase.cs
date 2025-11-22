using DonSagiv.Appl.Standard.DataAccess;
using DonSagiv.Domain.Standard.Entities;
using Microsoft.EntityFrameworkCore;

namespace DonSagiv.Infrastructure.DataAccess.EntityFramework.Sets;

public interface IDbSetBase : IRepositorySet
{

}

public interface IDbSetBase<TEntityModel> : IDbSetBase, IRepositorySet<TEntityModel>
    where TEntityModel : class, IEntityModel
{
    DbSet<TEntityModel> DbSet { get; }
}

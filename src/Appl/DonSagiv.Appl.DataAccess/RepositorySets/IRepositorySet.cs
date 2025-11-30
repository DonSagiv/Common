using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonSagiv.Appl.DataAccess.RepositorySets;

public interface IRepositorySet
{
    string SetName { get; }
}

public interface IRepositorySet<TEntityModel> : IRepositorySet
    where TEntityModel : IEntityModel
{
    Task<IResult> AppendAsync(TEntityModel entityModelInput);
    Task<IResult<TEntityModel>> ReadAsync(Ulid token);
    Task<IResult> DeleteAsync(Ulid token);
    IAsyncEnumerable<TEntityModel> AsQueryable();
}
using DonSagiv.Domain.Standard.DependencyInjection;
using DonSagiv.Domain.Standard.Entities;
using DonSagiv.Domain.Standard.ResultPattern;
using MongoDB.Driver;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Collections;

public abstract class MongoDbCollection : IMongoDbCollection
{
    #region Statics
    protected static readonly IError NotInitializedError = Error.FromDescription("MongoDB has not been initialized.");
    protected static readonly IError NotFoundError = Error.FromDescription("Entity not found in the collection.");
    #endregion

    #region Properties
    public required string SetName { get; set; }
    #endregion
}

[Export(typeof(IMongoDbCollection))]
internal class MongoDbCollection<TModel> : MongoDbCollection, IMongoDbCollection<TModel>
    where TModel : IEntityModel
{
    #region Fields
    private IMongoCollection<TModel>? _collection;
    #endregion

    #region Methods
    public async Task<IResult> AppendAsync(TModel entityModelInput)
    {
        if (_collection is null)
        {
            return Result.Failure(NotInitializedError);
        }

        try
        {
            await _collection.FindOneAndReplaceAsync(x => x.Id == entityModelInput.Id,
                entityModelInput,
                new FindOneAndReplaceOptions<TModel> { IsUpsert = true });

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public IAsyncEnumerable<TModel> AsQueryable()
    {
        if (_collection is null)
        {
            throw new Exception(NotInitializedError.Description);
        }

        return _collection.AsQueryable().ToAsyncEnumerable();
    }

    public async Task<IResult> DeleteAsync(Ulid token)
    {
        if (_collection is null)
        {
            return Result.Failure(NotInitializedError);
        }

        try
        {
            await _collection.DeleteOneAsync(x => x.Id == token);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<IResult<TModel>> ReadAsync(Ulid token)
    {
        if (_collection is null)
        {
            return Result.Failure<TModel>(NotInitializedError);
        }

        try
        {
            var model = await _collection
                .Find(x => x.Id == token)
                .FirstOrDefaultAsync();

            return model is null
                ? Result.Failure<TModel>(NotFoundError)
                : Result.Success(model);
        }
        catch(Exception ex) 
        {
            return Result.Failure<TModel>(ex);
        }
    }

    internal void SetCollection(IMongoCollection<TModel> collectionInput)
    {
        _collection = collectionInput;
    }
    #endregion
}

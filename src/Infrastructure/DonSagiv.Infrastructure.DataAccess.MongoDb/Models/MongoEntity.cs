using DonSagiv.Domain.Entities;
using MongoDB.Bson;

namespace DonSagiv.Infrastructure.DataAccess.MongoDb.Models;

internal abstract class MongoEntity : IEntityVersion
{
    public ObjectId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Version? CreatedVersion { get; set; }
    public DateTime LastSavedDate { get; set; }
    public Version? LastVersion { get; set; }
}
internal class MongoEntity<TModel> : MongoEntity
{
    public TModel? Data { get; set; }
}

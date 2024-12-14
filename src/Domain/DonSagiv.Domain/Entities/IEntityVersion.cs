namespace DonSagiv.Domain.Entities;

public interface IEntityVersion
{
    DateTime CreatedDate { get; set; }
    Version? CreatedVersion { get; set; }
    DateTime LastSavedDate { get; set; }
    Version? LastVersion { get; set; }
}
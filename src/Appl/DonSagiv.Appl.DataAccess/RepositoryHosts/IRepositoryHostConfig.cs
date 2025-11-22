namespace DonSagiv.Appl.DataAccess.RepositoryHosts;

public interface IRepositoryHostConfig
{
    public string DatabasePlatform { get; set; }
    public string ConnectionString { get; set; }
}

namespace DonSagiv.Domain.Extensions;

public static class FileExtensions
{
    #region Static Methods
    public static IEnumerable<string> GetFilesRecursive(string rootDirectory, Predicate<string>? filter = null)
    {
        foreach(var fileSystemEntry in Directory.EnumerateFiles(rootDirectory))
        {
            if (File.Exists(fileSystemEntry))
            {
                if(filter is null || filter.Invoke(fileSystemEntry))
                {
                    yield return fileSystemEntry;
                }
            }
            else if (Directory.Exists(fileSystemEntry))
            {
                foreach(var file in GetFilesRecursive(fileSystemEntry, filter))
                {
                    yield return file;
                }
            }
        }
    }
    #endregion
}

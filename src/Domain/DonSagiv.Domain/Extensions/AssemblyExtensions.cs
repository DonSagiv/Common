using System.Reflection;

namespace DonSagiv.Domain.Extensions;

public static class AssemblyExtensions
{
    #region Static Fields
    private static Predicate<string>? _defaultFilter = null;
    private static Assembly[]? _assemblyCache = null;
    #endregion

    #region Static Methods
    public static IEnumerable<Assembly> GetAssemblies(Predicate<string>? filter = null, params string[] externalDirectories)
    {
        if (_assemblyCache is not null)
        {
            return _assemblyCache;
        }

        var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => filter?.Invoke(x.FullName!) ?? true);

        var externalFiles = externalDirectories
            ?.SelectMany(x => FileExtensions.GetFilesRecursive(x))
            .Where(x => x.EndsWith(".dll") || x.EndsWith(".exe"))
            .Where(x => _defaultFilter?.Invoke(x) ?? true)
            .Where(x => filter?.Invoke(x) ?? true);

        _assemblyCache = (externalFiles ?? [])
            .SelectWhere((string x, out Assembly assembly) => TryLoadAssembly(x, out assembly))
            .Union(domainAssemblies)
            .ToArray();

        return _assemblyCache;
    }

    private static bool TryLoadAssembly(string assemblyPath, out Assembly assembly)
    {
        try
        {
            assembly = Assembly.LoadFrom(assemblyPath);

            return true;
        }
        catch (Exception)
        {
            assembly = null!;

            return false;
        }
    }

    public static string? GetExecutingAssemblyDirectory()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;

        var executingAssemblyDirectory = Path.GetDirectoryName(executingAssemblyPath);

        return executingAssemblyDirectory;
    }

    public static string[] AddExecutingAssemblyDirectory(this string[] assemblyDirectories)
    {
        var executingAssemblyDirectory = GetExecutingAssemblyDirectory();

        if (Directory.Exists(executingAssemblyDirectory))
        {
            assemblyDirectories = [.. assemblyDirectories, executingAssemblyDirectory];
        }

        return assemblyDirectories;
    }
    
    public static string? GetAssemblyProduct(this Type type)
    {
        return type.Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
    }

    public static string? GetAssemblyVersion(this Type type)
    {
        return type.Assembly.GetName().Version?.ToString();
    }
    #endregion
}

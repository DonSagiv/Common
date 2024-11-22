namespace DonSagiv.Domain.Entities;

public readonly record struct Version(uint Major, uint Minor, uint Release, uint Build)
{
    #region Static Methods
    public static Version FromString(string versionStringInput)
    {
        if (string.IsNullOrWhiteSpace(versionStringInput))
        {
            throw new ArgumentException("Version string is empty or null.", nameof(versionStringInput));
        }

        var versionSplit = versionStringInput
            .Split('.');

        if (versionSplit.Length > 4)
        {
            throw new ArgumentException("Version string is invalid: It must have at most 4 numbers.",
                nameof(versionStringInput));
        }

        var major = GetComponentFromSplit(versionSplit, 0);
        var minor = GetComponentFromSplit(versionSplit, 1);
        var release = GetComponentFromSplit(versionSplit, 2);
        var build = GetComponentFromSplit(versionSplit, 3);

        return new Version(major, minor, release, build);

        uint GetComponentFromSplit(string[] strings, int i)
        {
            if (i >= strings.Length)
            {
                return 0;
            }

            if (!uint.TryParse(strings[i], out var component))
            {
                throw new ArgumentException("Version components must be an unsigned integer.",
                    nameof(versionStringInput));
            }

            return component;
        }
    }
    #endregion

    #region Methods
    public override string ToString()
    {
        return string.Join(".", Major, Minor, Release, Build);
    }

    public string ToShortenedString()
    {
        var versionArray = new List<uint> { Major, Minor };

        if (Build > 0)
        {
            versionArray.Add(Release);
            versionArray.Add(Build);
        }
        else if (Release > 0)
        {
            versionArray.Add(Release);
        }

        return string.Join(".", versionArray);
    }
    #endregion
}
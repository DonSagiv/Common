using System.Diagnostics.CodeAnalysis;

namespace DonSagiv.Domain.Comparers;

public class KnuthMorrisPrattComparer : EqualityComparer<string>
{
    #region Methods
    public override bool Equals(string? filter, string? source)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return true;
        }

        if(string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(filter))
        {
            return false;
        }

        var filterCharArray = filter.ToLowerInvariant().ToCharArray();
        var sourceCharSpan = source!.ToLowerInvariant().AsSpan();

        var filterCharIndex = 0;

        foreach(var sourceChar in sourceCharSpan)
        {
            if(sourceChar == filterCharArray[filterCharIndex])
            {
                filterCharIndex++;
            }

            if(filterCharIndex >= filterCharArray.Length)
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}

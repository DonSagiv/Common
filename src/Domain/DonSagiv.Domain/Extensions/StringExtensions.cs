using System.Security.Cryptography;
using System.Text;

namespace DonSagiv.Domain.Extensions;

public static class StringExtensions
{
    #region Fields
    private static readonly HashAlgorithm hashAlgorithm = SHA512.Create();
    #endregion

    #region Static Methods
    public static byte[] GetHash(this string? valueToHash, string? startSalt, string? endSalt)
    {
        if (string.IsNullOrWhiteSpace(valueToHash))
        {
            return [];
        }

        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(startSalt))
        {
            sb.Append(startSalt);
        }

        sb.Append(valueToHash);

        if (!string.IsNullOrWhiteSpace(endSalt))
        {
            sb.Append(endSalt);
        }

        return hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes((sb.ToString())));
    }

    public static string ToCamelCase(this string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return string.Empty;
        }

        var inputLower = inputString
            .ToLower()
            .Trim();

        var validChars = inputLower
            .ToCharArray()
            .Where(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x))
            .ToList();

        var startOfWordIndices = validChars
            .Select((character, index) => new { character, index })
            .Where(x => char.IsWhiteSpace(x.character))
            .Select(x => x.index + 1)
            .ToList();

        foreach (var index in startOfWordIndices)
        {
            if (index < 0 || index >= validChars.Count)
            {
                continue;
            }

            // Convert start of word to upper case.
            validChars[index] = char.ToUpper(validChars[index]);
        }

        // Remove whitespace
        var finalCharArray = validChars
            .Where(char.IsLetterOrDigit)
            .ToArray();

        if (char.IsDigit(finalCharArray[0]))
        {
            finalCharArray = ['_', .. finalCharArray];
        }

        return new string(finalCharArray);
    }

    public static string RemoveAllWhitespace(this string inputString)
    {
        var nonWhiteSpaceChars = inputString
            .Where(x => !char.IsWhiteSpace(x))
            .ToArray();
        
        return new string(nonWhiteSpaceChars);
    }

    public static string RemoveAllDoubleSpace(this string inputString)
    {
        var inputCharArray = inputString.ToCharArray();
        var outputCharArray = new List<char>();
        var isSpace = false;

        foreach (var charValue in inputCharArray)
        {
            if(charValue == ' ')
            {
                if (isSpace)
                {
                    continue;
                }
                
                isSpace = true;
                outputCharArray.Add(charValue);
            }
            else
            {
                isSpace = false;
                outputCharArray.Add(charValue);
            }
        }
        
        return new string(outputCharArray.ToArray());
    }

    public static bool IsFileFriendly(this string sourceString)
    {
        return !sourceString
            .Intersect(Path.GetInvalidFileNameChars())
            .Any();
    }

    public static string? MakeFileFriendly(this string? sourceString)
    {
        if (sourceString is null)
        {
            return null;
        }
        
        return Path.GetInvalidFileNameChars()
            .Aggregate(sourceString, (current, character) => current.Replace(character, '_'));
    }

    public static string Wrap(this string input, int wrapLength = 100, char delimiter = '\n')
    {
        var charArray = input.ToCharArray()
            .Select((c, i) => (c, i))
            .ToArray();

        var charList = new List<char>();
        var previousChar = charArray[0];

        foreach (var currentChar in charArray)
        {
            if (IsNewLine(currentChar.c))
            {
                previousChar = currentChar;
            }

            if (!MeetsWrapCriteria(currentChar))
            {
                charList.Add(currentChar.c);

                continue;
            }
            
            charList.Add(delimiter);
            
            previousChar = currentChar; 
        }

        return new string(charList.ToArray());

        bool IsNewLine(char c)
        {
            return c is '\n' or '\r';
        }

        bool MeetsWrapCriteria((char c, int i) currentChar)
        {
            if (currentChar.i < 0)
            {
                return false;
            }
            
            return (currentChar.c is ' ' or '\t') && 
                   currentChar.i - previousChar.i >= wrapLength;
        }
    }
    #endregion
}

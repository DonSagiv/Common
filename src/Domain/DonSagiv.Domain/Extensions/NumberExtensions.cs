using System.Numerics;
using Convert = System.Convert;

namespace DonSagiv.Domain.Extensions;

public static class NumberExtensions
{
    #region Constants

    private const double TO_DEGREES = 180.0 / Math.PI;

    #endregion

    #region Methods

    public static bool IsEffectivelyZero(this double valueInput)
    {
        return Math.Abs(valueInput) < double.Epsilon;
    }

    public static int NumDigits(this int input)
    {
        return Math.Abs(input) <= 0
            ? 0
            : Convert.ToInt32(Math.Ceiling(Math.Log10(input)));
    }

    public static double ToDegrees(this double angleInputRadians)
    {
        return angleInputRadians * TO_DEGREES;
    }

    public static double ToRadians(this double angleInputDegrees)
    {
        return angleInputDegrees / TO_DEGREES;
    }

    public static bool IsWithinTolerance(this double? sourceValue, double? testValue, int toleranceDecimalPoints)
    {
        if (sourceValue is null && testValue is null)
        {
            return true;
        }

        if (sourceValue is null || testValue is null)
        {
            return false;
        }

        var tolerance = Math.Pow(10, -toleranceDecimalPoints);

        var difference = Math.Abs(sourceValue.Value - testValue.Value);

        return difference < tolerance;
    }

    public static double StandardDeviation<TNumber>(this IEnumerable<TNumber>? inputValues)
        where TNumber : INumber<TNumber>
    {
        if (inputValues is null)
        {
            return 0;
        }

        var valueList = inputValues.ToList();

        if (valueList.Count == 0)
        {
            return 0;
        }

        var average = valueList.Average(x => Convert.ToDouble(x));

        var sumMinusAverage = valueList.Sum(value => Math.Pow(Convert.ToDouble(value) - average, 2));

        return Math.Sqrt(sumMinusAverage / valueList.Count);
    }

    #endregion

    /// <summary>
    /// Finds the value and index of the closes value to the input value.
    /// </summary>
    /// <param name="enumerableInput">Enumerable to query against.</param>
    /// <param name="inputValue">Value to find closes to in array.</param>
    /// <returns>Item1 = First index of closest value in enumerable. Item2 = closest value in enumerable.</returns>
    private static (int index, TNumber value) FindClosest<TNumber>(this IEnumerable<TNumber> enumerableInput,
        TNumber inputValue)
        where TNumber : INumber<TNumber>
    {
        var enumerableArray = enumerableInput.ToArray();

        if (enumerableArray.Length == 0)
        {
            return (-1, TNumber.Zero);
        }

        if (enumerableArray.Length == 1)
        {
            return (0, enumerableArray[0]);
        }

        // Binary Search Algorithm
        var orderedEnumerable = enumerableArray
            .Select((x, i) => new { x, i })
            .GroupBy(x => x.x) // Group same values.
            .Select(x => new { x = x.Key, i = x.Min(y => y.i) }) // Get lowest index of same values.
            .OrderBy(x => x.x)
            .ToArray();

        var lowerIndex = 0;
        var upperIndex = orderedEnumerable.Length - 1;

        // While the two indices are not equal or adjacent to each other
        while (upperIndex - lowerIndex > 1)
        {
            // Get the midpoint of the two indices.
            var halfIndex = (upperIndex + lowerIndex) / 2;

            // Get the value at the midpoint.
            var halfIndexValue = orderedEnumerable[halfIndex].x;

            // If the midpoint is higher than the input value, return the lower half.
            if (inputValue > halfIndexValue)
            {
                lowerIndex = halfIndex;
            }
            // Otherwise return the upper half.
            else
            {
                upperIndex = halfIndex;
            }
        }

        // Return the value if the lower and upper index are equal.
        if (lowerIndex == upperIndex)
        {
            return (orderedEnumerable[lowerIndex].i, orderedEnumerable[lowerIndex].x);
        }

        // Otherwise see which value is closest to the input value and return its index.
        var upperDifference = orderedEnumerable[upperIndex].x - inputValue;
        var lowerDifference = inputValue - orderedEnumerable[lowerIndex].x;

        // Round up if value is halfway between two closes value.s
        return upperDifference <= lowerDifference
            ? (orderedEnumerable[upperIndex].i, orderedEnumerable[upperIndex].x)
            : (orderedEnumerable[lowerIndex].i, orderedEnumerable[lowerIndex].x);
    }

    public static int FindClosestIndex<TNumber>(this IEnumerable<TNumber> enumerableInput, TNumber inputValue)
        where TNumber : INumber<TNumber>
    {
        return enumerableInput.FindClosest(inputValue).index;
    }

    public static TNumber FindClosestValue<TNumber>(this IEnumerable<TNumber> enumerableInput, TNumber inputValue)
        where TNumber : INumber<TNumber>
    {
        return enumerableInput.FindClosest(inputValue).value;
    }

    public static double[] Normalize<TNumber>(this IEnumerable<TNumber> enumerableInput)
    {
        var inputArray = enumerableInput
            .Select(number => Convert.ToDouble(number))
            .ToArray();
        
        var min = inputArray.Min();
        var max = inputArray.Max();

        return inputArray
            .Select(x => (x - min) / (max - min))
            .ToArray();
    }
}
namespace DonSagiv.Domain.Standard.Extensions;

public static class NumberExtensions
{
    extension(double source)
    {
        public double Clamp(double MinValue = double.NegativeInfinity,
            double MaxValue = double.PositiveInfinity)
        {
            return source switch
            {
                _ when source < MinValue => MinValue,
                _ when source > MaxValue => MaxValue,
                _ => source,
            };
        }
    }

    extension(byte source)
    {
        public byte Clamp(byte MinValue = byte.MinValue,
            byte MaxValue = byte.MaxValue)
        {
            return source switch
            {
                _ when source < MinValue => MinValue,
                _ when source > MaxValue => MaxValue,
                _ => source,
            };
        }
    }
}

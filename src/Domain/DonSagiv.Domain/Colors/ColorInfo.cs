using System.Globalization;
using System.Text;

namespace DonSagiv.Domain.Colors;

public readonly record struct ColorInfo(byte A, byte R, byte G, byte B)
{
    #region Magic Numbers
    private const double _redLumCoeff = 0.2126;
    private const double _greenLumCoeff = 0.7152;
    private const double _blueLumCoeff = 0.0722;
    #endregion

    #region Constructor
    public ColorInfo(byte r, byte g, byte b) : this(byte.MaxValue, r, g, b) { }
    #endregion

    #region Methods
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append('#');
        sb.Append(A.ToString("X2"));
        sb.Append(R.ToString("X2"));
        sb.Append(G.ToString("X2"));
        sb.Append(B.ToString("X2"));

        return sb.ToString();
    }

    public string ToRgbString()
    {
        var sb = new StringBuilder();

        sb.Append('#');
        sb.Append(R.ToString("X2"));
        sb.Append(G.ToString("X2"));
        sb.Append(B.ToString("X2"));

        return sb.ToString();
    }

    public (byte a, byte r, byte b, byte g) ToArgb()
    {
        return (A, R, G, B);
    }

    public (byte r, byte b, byte g) ToRgb()
    {
        return (R, G, B);
    }

    public int Encode()
    {
        var colorData = A << 24;
        colorData |= R << 16;
        colorData |= G << 8;
        colorData |= B << 0;

        return colorData;
    }
    #endregion

    #region Static Methods
    public static ColorInfo FromHexColorString(string colorString)
    {
        if (string.IsNullOrWhiteSpace(colorString) || colorString[0] != '#')
        {
            return default;
        }

        if(colorString.Length == 7)
        {
            if(!byte.TryParse(colorString.AsSpan(1, 2), NumberStyles.HexNumber, null, out var r) ||
                    !byte.TryParse(colorString.AsSpan(3, 4), NumberStyles.HexNumber, null, out var g) ||
                    !byte.TryParse(colorString.AsSpan(5, 6), NumberStyles.HexNumber, null, out var b))
            {
                return default;
            }

            return new ColorInfo(r, g, b);
        }

        if(colorString.Length == 9)
        {
            if (!byte.TryParse(colorString.AsSpan(1, 2), NumberStyles.HexNumber, null, out var a) ||
                    !byte.TryParse(colorString.AsSpan(3, 4), NumberStyles.HexNumber, null, out var r) ||
                    !byte.TryParse(colorString.AsSpan(5, 6), NumberStyles.HexNumber, null, out var g) ||
                    !byte.TryParse(colorString.AsSpan(7, 8), NumberStyles.HexNumber, null, out var b))
            {
                return default;
            }

            return new ColorInfo(a, r, g, b);
        }

        return default;
    }

    public double GetLuminance()
    {
        var vR = Convert.ToDouble(R) / byte.MaxValue;
        var vG = Convert.ToDouble(G) / byte.MaxValue;
        var vB = Convert.ToDouble(B) / byte.MaxValue;

        return _redLumCoeff * vR + _greenLumCoeff * vG + _blueLumCoeff * vB;
    }

    public ColorInfo ModifyLuminanceBy(int value)
    {
        var r = Math.Clamp(Convert.ToByte((value + R)), byte.MinValue, byte.MaxValue);
        var g = Math.Clamp(Convert.ToByte((value + G)), byte.MinValue, byte.MaxValue);
        var b = Math.Clamp(Convert.ToByte((value + B)), byte.MinValue, byte.MaxValue);

        return new ColorInfo(r, g, b);
    }
    #endregion
}

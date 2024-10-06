namespace DonSagiv.Domain.Colors;

public static class ColorGenerator
{
    #region Magic Numbers
    private const double _redLumCoeff = 0.2126;
    private const double _greenLumCoeff = 0.7152;
    private const double _blueLumCoeff = 0.0722;
    #endregion

    #region Fields
    private static readonly Random _random = new Random();
    private static int _colorIndex;
    private static ColorInfo _currentColor;
    private static readonly IList<ColorInfo> _colorList =
    [
        ColorInfo.FromHexColorString("#FFFF7F7F"), // Light Red
        ColorInfo.FromHexColorString("#FF7FFF7F"), // Light Green
        ColorInfo.FromHexColorString("#FF7F7FFF"), // Light Blue
        ColorInfo.FromHexColorString("#FFFFFF7F"), // Light Yellow
        ColorInfo.FromHexColorString("#FF7FFFFF"), // Light Cyan
        ColorInfo.FromHexColorString("#FFFF7FFF"), // Light Magenta
        ColorInfo.FromHexColorString("#FFFFBF7F"), // Light Orange
        ColorInfo.FromHexColorString("#FFBF7FFF"), // Light Purple
        ColorInfo.FromHexColorString("#FF7FBFFF"), // Light Sky Blue
        ColorInfo.FromHexColorString("#FFFF7FBF"), // Light Pink
        ColorInfo.FromHexColorString("#FF7FFF7F"), // Light Lime Green
        ColorInfo.FromHexColorString("#FFBF7F7F"), // Light Brown
        ColorInfo.FromHexColorString("#FF7FFFF7"), // Light Aquamarine
        ColorInfo.FromHexColorString("#FFFFF77F"), // Light Gold
        ColorInfo.FromHexColorString("#FF7FF7FF"), // Light Powder Blue
        ColorInfo.FromHexColorString("#FFFF7FF7"), // Light Hot Pink
        ColorInfo.FromHexColorString("#FF7FFF7F"), // Light Chartreuse
        ColorInfo.FromHexColorString("#FFBFFF7F"), // Light Olive
        ColorInfo.FromHexColorString("#FF7FBFFF"), // Light Steel Blue
        ColorInfo.FromHexColorString("#FFFFBF7F"), // Light Coral
        ColorInfo.FromHexColorString("#FF7FFF7F"), // Light Sea Green
        ColorInfo.FromHexColorString("#FF7F7FFF"), // Light Slate Blue
        ColorInfo.FromHexColorString("#FFFF7FFF"), // Light Violet
        ColorInfo.FromHexColorString("#FF7F7F7F"), // Gray
    ];
    #endregion

    #region Methods
    public static ColorInfo Current()
    {
        return _currentColor;
    }

    public static ColorInfo Next()
    {
        var colorInfo = _colorList[_colorIndex++];

        if (_colorIndex >= _colorList.Count)
        {
            _colorIndex = 0;
        }

        _currentColor = colorInfo;

        return colorInfo;
    }

    public static void Reset()
    {
        _colorIndex = 0;
        _currentColor = _colorList[0];
    }

    public static ColorInfo Random(LightnessFilter lightnessFilter = LightnessFilter.None,
        HueFilter hueFilter = HueFilter.None)
    {
        var (minLightness, maxLightness) = GetLightnessRange(lightnessFilter);
        var lightness = _random.NextDouble() * (maxLightness - minLightness) + minLightness;

        var (minHue, maxHue) = GetHueRange(hueFilter);
        var hue = _random.NextDouble() * (minHue - maxHue) + maxHue;

        if (hue < 0)
        {
            hue += 360;
        }

        // Saturation should always be above 0.5 (not gray) and below 0.9 (not too vibrant)
        var saturation = _random.NextDouble() * 0.4 + 0.5;

        // Calculate HSL color values.
        var d = saturation * (1 - Math.Abs(2 * lightness - 1));
        var m = byte.MaxValue * (lightness - 0.5 * d);
        var x = d * (1 - Math.Abs(hue / 60 % 2 - 1));

        return hue switch
        {
            _ when hue <= 60 =>     new ColorInfo(Convert.ToByte(byte.MaxValue * d + m), Convert.ToByte(byte.MaxValue * x + m), Convert.ToByte(m)),
            _ when hue <= 120 =>    new ColorInfo(Convert.ToByte(byte.MaxValue * x + m), Convert.ToByte(byte.MaxValue * d + m), Convert.ToByte(m)),
            _ when hue <= 180 =>    new ColorInfo(Convert.ToByte(m), Convert.ToByte(byte.MaxValue * d + m), Convert.ToByte(byte.MaxValue * x + m)),
            _ when hue <= 240 =>    new ColorInfo(Convert.ToByte(m), Convert.ToByte(byte.MaxValue * x + m), Convert.ToByte(byte.MaxValue * d + m)),
            _ when hue <= 300 =>    new ColorInfo(Convert.ToByte(byte.MaxValue * x + m), Convert.ToByte(m), Convert.ToByte(byte.MaxValue * d + m)),
            _ =>                    new ColorInfo(Convert.ToByte(byte.MaxValue * d + m), Convert.ToByte(m), Convert.ToByte(byte.MaxValue * x + m)),
        };
    }

    private static (double, double) GetLightnessRange(LightnessFilter filter)
    {
        return filter switch
        {
            LightnessFilter.None => (0.3, 0.9),
            LightnessFilter.Bright => (0.7, 0.9),
            LightnessFilter.Medium => (0.5, 0.7),
            LightnessFilter.Dark => (0.3, 0.5),
            _ => throw new NotImplementedException()
        };
    }

    private static (double, double) GetHueRange(HueFilter filter)
    {
        return filter switch
        {
            HueFilter.None => (0, 360),
            HueFilter.Red => (-15, 15),
            HueFilter.Orange => (25, 40),
            HueFilter.Yellow => (45, 65),
            HueFilter.Green => (80, 145),
            HueFilter.Cyan => (175, 190),
            HueFilter.Blue => (200, 240),
            HueFilter.Purple => (250, 290),
            HueFilter.Magenta => (300, 335),
            _ => throw new NotImplementedException()
        };
    }
    #endregion
}

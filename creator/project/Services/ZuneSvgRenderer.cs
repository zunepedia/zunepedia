using Zunepedia.Creator.Models;

namespace Zunepedia.Creator.Services;

/// <summary>
/// Renders the selected Zune device family as a self-contained SVG image.
/// </summary>
public static partial class ZuneSvgRenderer
{
    /// <summary>
    /// Renders a Zune device SVG for the specified family, shell colour, and output scale.
    /// </summary>
    /// <param name="type">The device family geometry to render.</param>
    /// <param name="color">The shell colour as a CSS-compatible colour value.</param>
    /// <param name="scale">The multiplier applied to the SVG view box dimensions.</param>
    /// <param name="cropToDevice">Whether to crop the SVG canvas to the device body.</param>
    /// <param name="embedScreenLogo">Whether to embed the screen logo in the SVG.</param>
    /// <returns>A complete SVG document fragment suitable for display or download.</returns>
    public static string Render(
        ZuneType type,
        string color,
        double scale = 12,
        bool cropToDevice = false,
        bool embedScreenLogo = true)
    {
        var deviceBounds = GetViewBox(type);
        var viewBox = cropToDevice
            ? new ViewBox(0, 0, deviceBounds.Width, deviceBounds.Height)
            : new ViewBox(0, 0, 21, 36);
        var width = Format(viewBox.Width * scale);
        var height = Format(viewBox.Height * scale);
        var body = RenderBody(type, color, embedScreenLogo);

        if (cropToDevice && (deviceBounds.X != 0 || deviceBounds.Y != 0))
        {
            body = $"""
                <g transform="translate({Format(-deviceBounds.X)} {Format(-deviceBounds.Y)})">
                {body}
                </g>
                """;
        }

        return $$"""
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="{{Format(viewBox.X)}} {{Format(viewBox.Y)}} {{Format(viewBox.Width)}} {{Format(viewBox.Height)}}" width="{{width}}" height="{{height}}" role="img" aria-label="{{GetTitle(type)}}">
              <defs>
                {{RenderDefinitions(type)}}
              </defs>
              {{body}}
            </svg>
            """;
    }

    private static string RenderDefinitions(ZuneType type) => type switch
    {
        ZuneType.Keel => $$"""
              {{ScreenGradient}}
              <linearGradient id="zune-body-keel" x1="0" y1="0" x2="0" y2="1">
                <stop offset="0" stop-color="#020202" />
                <stop offset="1" stop-color="#424242" />
              </linearGradient>
            """,
        ZuneType.Draco or ZuneType.Scorpius => $$"""
              {{ScreenGradient}}
              <linearGradient id="zune-pad" x1="0" y1="0" x2="0" y2="1">
                <stop offset="0" stop-color="#000000" stop-opacity="0.4" />
                <stop offset="1" stop-color="#000000" stop-opacity="0.13" />
              </linearGradient>
              <linearGradient id="zune-pad-outer" x1="0" y1="0" x2="0" y2="1">
                <stop offset="0" stop-color="#FFFFFF" stop-opacity="0.4" />
                <stop offset="1" stop-color="#FFFFFF" stop-opacity="0.13" />
              </linearGradient>
            """,
        ZuneType.Pavo => $$"""
              {{ScreenGradient}}
              <linearGradient id="zune-hd-button" x1="0" y1="0" x2="0" y2="1">
                <stop offset="0" stop-color="#162025" />
                <stop offset="1" stop-color="#0C1417" />
              </linearGradient>
            """,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    private const string ScreenGradient = """
        <linearGradient id="zune-screen" x1="0" y1="1" x2="1" y2="0">
          <stop offset="0" stop-color="#524F4F" />
          <stop offset="1" stop-color="#231F20" />
        </linearGradient>
        """;

    /// <summary>
    /// Gets the tightly cropped output dimensions for a device at the specified scale.
    /// </summary>
    /// <param name="type">The device family geometry to measure.</param>
    /// <param name="scale">The multiplier applied to the device bounds.</param>
    /// <returns>The integer pixel dimensions required to contain the device.</returns>
    public static (int Width, int Height) GetPixelDimensions(ZuneType type, double scale)
    {
        var dimensions = GetDimensions(type);
        return (
            (int)Math.Ceiling(dimensions.Width * scale),
            (int)Math.Ceiling(dimensions.Height * scale));
    }

    /// <summary>
    /// Gets the tightly cropped dimensions of a device in SVG user units.
    /// </summary>
    /// <param name="type">The device family geometry to measure.</param>
    /// <returns>The width and height of the device bounds.</returns>
    public static (double Width, double Height) GetDimensions(ZuneType type)
    {
        var viewBox = GetViewBox(type);
        return (viewBox.Width, viewBox.Height);
    }

    private static ViewBox GetViewBox(ZuneType type) => type switch
    {
        ZuneType.Keel => new(0.8, 0.6, 19.4, 34.8),
        ZuneType.Draco => new(0, 0, 20, 36),
        ZuneType.Scorpius => new(0, 0, 14, 30),
        ZuneType.Pavo => new(0, 0, 16, 32),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    private static string GetTitle(ZuneType type) => type switch
    {
        ZuneType.Keel => "Zune 30",
        ZuneType.Draco => "Zune 80 | 120",
        ZuneType.Scorpius => "Zune 4 | 8 | 16",
        ZuneType.Pavo => "Zune HD",
        _ => "Zune"
    };

    private readonly record struct ViewBox(double X, double Y, double Width, double Height);
}

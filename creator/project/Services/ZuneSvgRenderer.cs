using Zunepedia.Creator.Models;

namespace Zunepedia.Creator.Services;

/// <summary>
/// Renders the selected Zune device family as a self-contained SVG image.
/// </summary>
public static partial class ZuneSvgRenderer
{
    /// <summary>
    /// The renderer view box width in SVG user units.
    /// </summary>
    public const int Width = 21;

    /// <summary>
    /// The renderer view box height in SVG user units.
    /// </summary>
    public const int Height = 36;

    /// <summary>
    /// Renders a Zune device SVG for the specified family, shell colour, and output scale.
    /// </summary>
    /// <param name="type">The device family geometry to render.</param>
    /// <param name="color">The shell colour as a CSS-compatible colour value.</param>
    /// <param name="scale">The multiplier applied to the SVG view box dimensions.</param>
    /// <returns>A complete SVG document fragment suitable for display or download.</returns>
    public static string Render(ZuneType type, string color, double scale = 12)
    {
        var width = Format(Width * scale);
        var height = Format(Height * scale);

        return $$"""
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 21 36" width="{{width}}" height="{{height}}" role="img" aria-label="{{GetTitle(type)}}">
              <defs>
                <linearGradient id="zune-body-keel" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0" stop-color="#020202" />
                  <stop offset="1" stop-color="#424242" />
                </linearGradient>
                <linearGradient id="zune-pad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0" stop-color="#000000" stop-opacity="0.4" />
                  <stop offset="1" stop-color="#000000" stop-opacity="0.13" />
                </linearGradient>
                <linearGradient id="zune-pad-outer" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0" stop-color="#FFFFFF" stop-opacity="0.4" />
                  <stop offset="1" stop-color="#FFFFFF" stop-opacity="0.13" />
                </linearGradient>
                <linearGradient id="zune-screen" x1="0" y1="1" x2="1" y2="0">
                  <stop offset="0" stop-color="#524F4F" />
                  <stop offset="1" stop-color="#231F20" />
                </linearGradient>
                <linearGradient id="zune-hd-button" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0" stop-color="#162025" />
                  <stop offset="1" stop-color="#0C1417" />
                </linearGradient>
              </defs>
              {{RenderBody(type, color)}}
            </svg>
            """;
    }

    private static string GetTitle(ZuneType type) => type switch
    {
        ZuneType.Keel => "Zune 30",
        ZuneType.Draco => "Zune 80 | 120",
        ZuneType.Scorpius => "Zune 4 | 8 | 16",
        ZuneType.Pavo => "Zune HD",
        _ => "Zune"
    };
}

using SkiaSharp;
using Svg.Skia;
using Zunepedia.Creator.Models;

namespace Zunepedia.Creator.Services;

/// <summary>
/// Rasterizes generated Zune SVG markup into transparent PNG data URIs.
/// </summary>
public static class ZunePngRenderer
{
    private static readonly Dictionary<CacheKey, CacheEntry> Cache = [];

    /// <summary>
    /// Renders SVG markup as a cached PNG data URI at the requested dimensions.
    /// </summary>
    /// <param name="type">The device family geometry to render.</param>
    /// <param name="color">The shell colour as a CSS-compatible colour value.</param>
    /// <param name="width">The PNG width in pixels.</param>
    /// <param name="height">The PNG height in pixels.</param>
    /// <param name="isCustom">Whether this image should use the replaceable Custom cache slot.</param>
    /// <returns>A Base64-encoded PNG data URI suitable for an HTML download link.</returns>
    public static string RenderDataUri(
        ZuneType type,
        string color,
        int width,
        int height,
        bool isCustom = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(color);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);

        var normalizedColor = color.ToUpperInvariant();
        var key = new CacheKey(
            type,
            isCustom ? "Custom" : normalizedColor,
            width,
            height);

        if (Cache.TryGetValue(key, out var cached) &&
            cached.Color == normalizedColor)
        {
            return cached.DataUri;
        }

        var dimensions = ZuneSvgRenderer.GetDimensions(type);
        var scale = Math.Min(width / dimensions.Width, height / dimensions.Height);
        var svgMarkup = ZuneSvgRenderer.Render(
            type,
            normalizedColor,
            scale,
            cropToDevice: true);

        using var svg = new SKSvg();
        var picture = svg.FromSvg(svgMarkup)
            ?? throw new InvalidOperationException("Generated Zune SVG could not be parsed.");

        var imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(imageInfo)
            ?? throw new InvalidOperationException("PNG drawing surface could not be created.");

        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Transparent);
        var bounds = picture.CullRect;
        if (bounds.IsEmpty)
        {
            throw new InvalidOperationException("Generated Zune SVG has no drawable bounds.");
        }

        canvas.Scale(width / bounds.Width, height / bounds.Height);
        canvas.Translate(-bounds.Left, -bounds.Top);
        canvas.DrawPicture(picture);
        canvas.Flush();

        using var image = surface.Snapshot();
        using var png = image.Encode(SKEncodedImageFormat.Png, 100)
            ?? throw new InvalidOperationException("Rendered Zune image could not be encoded as PNG.");

        var dataUri = $"data:image/png;base64,{Convert.ToBase64String(png.ToArray())}";
        Cache[key] = new CacheEntry(normalizedColor, dataUri);
        return dataUri;
    }

    private readonly record struct CacheKey(
        ZuneType Type,
        string Style,
        int Width,
        int Height);

    private sealed record CacheEntry(string Color, string DataUri);
}

namespace Zunepedia.Creator.Models;

/// <summary>
/// Describes a selectable colour style and the Zune device families that support it.
/// </summary>
/// <param name="Name">The display name shown for the colour.</param>
/// <param name="Color">The device shell colour as a CSS-compatible hex value.</param>
/// <param name="Types">The device families that can use this colour.</param>
public sealed record ZuneStyle(string Name, string Color, IReadOnlySet<ZuneType> Types);


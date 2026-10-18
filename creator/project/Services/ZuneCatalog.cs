using Zunepedia.Creator.Models;

namespace Zunepedia.Creator.Services;

/// <summary>
/// Provides the supported Zune devices and colour styles for the Creator UI.
/// </summary>
public sealed class ZuneCatalog
{
    /// <summary>
    /// Gets the supported Zune device models in their display order.
    /// </summary>
    public IReadOnlyList<ZuneDevice> Devices { get; } =
    [
        new("Zune 30", "Keel", ZuneType.Keel, "#443016"),
        new("Zune 80 | 120", "Draco", ZuneType.Draco, "#1C1C1C"),
        new("Zune 4 | 8 | 16", "Scorpius", ZuneType.Scorpius, "#84142C"),
        new("Zune HD", "Pavo", ZuneType.Pavo, "#a59d95")
    ];

    /// <summary>
    /// Gets the supported colour styles across all device families.
    /// </summary>
    public IReadOnlyList<ZuneStyle> Styles { get; } =
    [
        Style("Brown", "#443016", ZuneType.Keel),
        Style("Halo", "#465a5a", ZuneType.Keel),
        Style("Red", "#dd5459", ZuneType.Keel),
        Style("Pink", "#e6b8c5", ZuneType.Keel),
        Style("Orange", "#F67400", ZuneType.Keel),
        Style("Black", "#101816", ZuneType.Keel),
        Style("White", "#e8e9e8", ZuneType.Keel),
        Style("Black", "#1c1c1c", ZuneType.Draco),
        Style("Red", "#7b142c", ZuneType.Draco),
        Style("Blue", "#024682", ZuneType.Draco),
        Style("White", "#eaeaea", ZuneType.Draco),
        Style("Red", "#84142c", ZuneType.Scorpius),
        Style("Blue", "#024682", ZuneType.Scorpius),
        Style("Green", "#8c8442", ZuneType.Scorpius),
        Style("Pink", "#f6a4c4", ZuneType.Scorpius),
        Style("Citron", "#d2c606", ZuneType.Scorpius),
        Style("White", "#eaeaea", ZuneType.Scorpius),
        Style("Black", "#1c1c1c", ZuneType.Scorpius),
        Style("Platinum", "#a59d95", ZuneType.Pavo),
        Style("Black", "#162124", ZuneType.Pavo),
        Style("Red", "#7c1e40", ZuneType.Pavo),
        Style("Blue", "#1e6892", ZuneType.Pavo),
        Style("Green", "#a6ac5c", ZuneType.Pavo),
        Style("Pink", "#c0a2b6", ZuneType.Pavo),
        Style("Magenta", "#8c588c", ZuneType.Pavo),
        Style("Purple", "#5e5a78", ZuneType.Pavo),
        Style("Atomic", "#ba1800", ZuneType.Pavo)
    ];

    /// <summary>
    /// Gets the colour styles supported by the specified device family.
    /// </summary>
    /// <param name="type">The device family to filter styles for.</param>
    /// <returns>The styles available for <paramref name="type"/>.</returns>
    public IEnumerable<ZuneStyle> GetStyles(ZuneType type) =>
        Styles.Where(style => style.Types.Contains(type));

    private static ZuneStyle Style(string name, string color, params ZuneType[] types) =>
        new(name, color, types.ToHashSet());
}

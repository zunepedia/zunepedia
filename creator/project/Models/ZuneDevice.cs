namespace Zunepedia.Creator.Models;

/// <summary>
/// Describes a selectable Zune device model and its default appearance.
/// </summary>
/// <param name="Name">The display name shown in the Creator UI.</param>
/// <param name="CodeName">The device family code name used in labels and exported filenames.</param>
/// <param name="Type">The renderer geometry used for this device.</param>
/// <param name="DefaultColor">The default device shell colour as a CSS-compatible hex value.</param>
public sealed record ZuneDevice(string Name, string CodeName, ZuneType Type, string DefaultColor);

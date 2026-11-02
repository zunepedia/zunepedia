using Microsoft.AspNetCore.Components;
using Zunepedia.Creator.Models;
using Zunepedia.Creator.Services;

namespace Zunepedia.Creator.Pages;

public partial class Home
{
    private const double ExportScale = 24;
    private const int PngScale = 48;
    private const string CustomStyleName = "Custom";

    [Inject]
    private ZuneCatalog Catalog { get; set; } = null!;

    private ZuneType SelectedType { get; set; } = ZuneType.Keel;

    private string SelectedStyleName { get; set; } = "Brown";

    private Dictionary<ZuneType, string> CustomColors { get; } = [];

    private string? PngDataUri { get; set; }

    private string? PngError { get; set; }

    private bool IsPngRendering { get; set; }

    private ZuneDevice SelectedDevice =>
        Catalog.Devices.Single(device => device.Type == SelectedType);

    private IReadOnlyList<ZuneStyle> AvailableStyles =>
        [.. Catalog.GetStyles(SelectedType)];

    private bool IsCustomSelected => SelectedStyleName == CustomStyleName;

    private string CustomColor =>
        CustomColors.TryGetValue(SelectedType, out var color)
            ? color
            : SelectedDevice.DefaultColor;

    private string SelectedColor =>
        IsCustomSelected
            ? CustomColor
            : AvailableStyles.FirstOrDefault(style => style.Name == SelectedStyleName)?.Color
                ?? AvailableStyles[0].Color;

    private string SvgMarkup =>
        ZuneSvgRenderer.Render(
            SelectedType,
            SelectedColor,
            ExportScale,
            cropToDevice: true);

    private string SvgDataUri =>
        $"data:image/svg+xml;charset=utf-8,{Uri.EscapeDataString(SvgMarkup)}";

    private string FileNameStem =>
        $"{SelectedStyleName}-{SelectedDevice.CodeName}".Replace(' ', '-').ToLowerInvariant();

    private string SvgFileName => $"{FileNameStem}.svg";

    private string PngFileName => $"{FileNameStem}.png";

    private (int Width, int Height) PngDimensions =>
        ZuneSvgRenderer.GetPixelDimensions(SelectedType, PngScale);

    protected override void OnInitialized()
    {
        foreach (var device in Catalog.Devices)
        {
            CustomColors.Add(device.Type, device.DefaultColor);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await PreparePngAsync();
        }
    }

    private async Task SelectDevice(ZuneType type)
    {
        SelectedType = type;
        SelectedStyleName = AvailableStyles[0].Name;
        await PreparePngAsync();
    }

    private async Task SelectStyle(string name)
    {
        SelectedStyleName = name;
        await PreparePngAsync();
    }

    private async Task SelectCustomStyle()
    {
        SelectedStyleName = CustomStyleName;
        await PreparePngAsync();
    }

    private void PreviewCustomColor(ChangeEventArgs args)
    {
        if (!TryGetColor(args.Value, out var color))
        {
            return;
        }

        CustomColors[SelectedType] = color;
        SelectedStyleName = CustomStyleName;
        PngDataUri = null;
        PngError = null;
        IsPngRendering = true;
    }

    private async Task CommitCustomColor(ChangeEventArgs args)
    {
        if (!TryGetColor(args.Value, out var color))
        {
            return;
        }

        CustomColors[SelectedType] = color;
        SelectedStyleName = CustomStyleName;
        await PreparePngAsync();
    }

    private async Task PreparePngAsync()
    {
        PngDataUri = null;
        PngError = null;
        IsPngRendering = true;
        await InvokeAsync(StateHasChanged);
        await Task.Yield();

        try
        {
            var dimensions = PngDimensions;
            PngDataUri = ZunePngRenderer.RenderDataUri(
                SelectedType,
                SelectedColor,
                dimensions.Width,
                dimensions.Height,
                IsCustomSelected);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            PngError = "PNG could not be generated. SVG download available.";
        }
        finally
        {
            IsPngRendering = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private static bool TryGetColor(object? value, out string color)
    {
        var candidate = value?.ToString();
        if (candidate is { Length: 7 } &&
            candidate[0] == '#' &&
            candidate[1..].All(char.IsAsciiHexDigit))
        {
            color = candidate;
            return true;
        }

        color = string.Empty;
        return false;
    }
}

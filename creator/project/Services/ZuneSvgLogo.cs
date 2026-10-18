using System.Reflection;
using System.Text;

namespace Zunepedia.Creator.Services;

public static partial class ZuneSvgRenderer
{
    private const string ScreenLogoResourceName = "Zunepedia.Creator.Assets.zunepedia-screen-logo.svg";
    private static readonly Lazy<string> ScreenLogoDataUri = new(LoadScreenLogoDataUri);

    private static string RenderScreenLogo(double screenX, double screenY, double screenWidth, double screenHeight, double heightRatio = 0.6)
    {
        const double logoAspectRatio = 1050d / 1200d;

        var logoHeight = screenHeight * heightRatio;
        var logoWidth = logoHeight * logoAspectRatio;
        var maxLogoWidth = screenWidth * 0.85;

        if (logoWidth > maxLogoWidth)
        {
            logoWidth = maxLogoWidth;
            logoHeight = logoWidth / logoAspectRatio;
        }

        var x = screenX + ((screenWidth - logoWidth) / 2);
        var y = screenY + ((screenHeight - logoHeight) / 2);

        return $"""<image href="{ScreenLogoDataUri.Value}" x="{Format(x)}" y="{Format(y)}" width="{Format(logoWidth)}" height="{Format(logoHeight)}" opacity="0.9" preserveAspectRatio="xMidYMid meet" />""";
    }

    private static string LoadScreenLogoDataUri()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(ScreenLogoResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ScreenLogoResourceName}' was not found.");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var svg = reader.ReadToEnd();
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(svg));
        return $"data:image/svg+xml;base64,{base64}";
    }
}
using System.Globalization;
using Zunepedia.Creator.Models;

namespace Zunepedia.Creator.Services;

public static partial class ZuneSvgRenderer
{
    private static string RenderBody(ZuneType type, string color) => type switch
    {
        ZuneType.Keel => $$"""
              <rect x="0.8" y="0.6" width="19.4" height="34.8" rx="0.85" ry="0.85" fill="{{color}}" />
              <rect x="1.75" y="1.55" width="17.5" height="23.2" rx="0.35" ry="0.35" fill="#666867" />
              <rect x="2.65" y="2.45" width="15.7" height="21.4" fill="url(#zune-screen)" />
              {{RenderScreenLogo(2.65, 2.45, 15.7, 21.4)}}
              <ellipse cx="10.5" cy="29.35" rx="5.45" ry="5.25" fill="#666867" />
              <ellipse cx="10.5" cy="29.35" rx="4.05" ry="3.85" fill="url(#zune-body-keel)" />
            """,
        ZuneType.Draco => $$"""
              <rect x="0" y="0" width="20" height="36" rx="1" ry="1" fill="{{color}}" />
              <rect x="1" y="1" width="18" height="24" rx="0.35" ry="0.35" fill="url(#zune-screen)" stroke="#191616" />
              {{RenderScreenLogo(1, 1, 18, 24, 0.535)}}
              <path d="M 9.43 26.5 h 1.14 c 1.9 0 3.43 1.53 3.43 3.43 v 1.14 c 0 1.9 -1.53 3.43 -3.43 3.43 H 9.43 C 7.53 34.5 6 32.97 6 31.07 v -1.14 c 0 -1.9 1.53 -3.43 3.43 -3.43 z" fill="url(#zune-pad)" stroke="url(#zune-pad-outer)" stroke-width="1" />
            """,
        ZuneType.Scorpius => $$"""
              <rect x="0" y="0" width="14" height="30" rx="1.2" ry="1.2" fill="{{color}}" />
              <rect x="1" y="1" width="12" height="15" rx="0.35" ry="0.35" fill="#191616" />
              <rect x="2.25" y="2.75" width="9.5" height="11.5" fill="url(#zune-screen)" />
              {{RenderScreenLogo(2.25, 2.75, 9.5, 11.5)}}
              <path d="M 6.25 19 h 1.5 c 2.08 0 3.75 1.67 3.75 3.75 v 1.5 c 0 2.08 -1.67 3.75 -3.75 3.75 h -1.5 c -2.08 0 -3.75 -1.67 -3.75 -3.75 v -1.5 C 2.5 20.67 4.17 19 6.25 19 z" fill="url(#zune-pad)" stroke="url(#zune-pad-outer)" />
            """,
        ZuneType.Pavo => $$"""
              <rect x="0" y="0" width="16" height="32" rx="1.2" ry="1.2" fill="{{color}}" />
              <polygon points="4,27 12,27 10,28.5 6,28.5" fill="url(#zune-hd-button)" />
              <path d="M 1 27 V 1.35 C 1 1.16 1.16 1 1.35 1 h 13.3 C 14.84 1 15 1.16 15 1.35 V 27 Z" fill="#000000" stroke="#000000" stroke-width="1" />
              <rect x="1.5" y="1.5" width="13" height="23.5" fill="url(#zune-screen)" />
              {{RenderScreenLogo(1.5, 1.5, 13, 23.5, 0.46)}}
            """,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    private static string Format(double value) => value.ToString("0.##", CultureInfo.InvariantCulture);
}
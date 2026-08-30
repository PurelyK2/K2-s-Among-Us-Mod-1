using MiraAPI.Utilities;
using TouExtensionExample.Roles.Neutral;
using TownOfUs;
using UnityEngine;

namespace TouExtensionExample;

public static class TouExampleColors
{
    // Crew Colors
    public static Color Child => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(255, 127, 0, byte.MaxValue);
    public static Color Scrubber => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(97, 147, 212, byte.MaxValue);
    // Neutral Colors
    public static Color Mimic => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(75, 219, 106, byte.MaxValue);
}
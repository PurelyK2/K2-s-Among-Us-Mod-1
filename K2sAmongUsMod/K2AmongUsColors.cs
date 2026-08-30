using MiraAPI.Utilities;
using TownOfUs;
using UnityEngine;

namespace K2AmongUs;

public static class K2AmongUsColors
{
    // Crew Colors
    public static Color Child => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(255, 127, 0, byte.MaxValue);
    public static Color Scrubber => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(97, 147, 212, byte.MaxValue);
    // Neutral Colors
    public static Color Mimic => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(75, 219, 106, byte.MaxValue);
}
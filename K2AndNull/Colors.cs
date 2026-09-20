using MiraAPI.Utilities;
using TownOfUs;
using UnityEngine;

namespace K2AndNull;

/// <inheritdoc/>
public static class Colors
{
    // Crew Colors
    public static Color Gossip => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(255, 237, 162, byte.MaxValue);
    public static Color JackOfAll => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : Color.white;
    public static Color Snoop => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(97, 147, 212, byte.MaxValue);

    //Neutral Colors
    public static Color Scrubber => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(97, 147, 212, byte.MaxValue);
    public static Color BountyHunter => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(10, 47, 14, byte.MaxValue);
    public static Color Forbearing => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(230, 242, 200, byte.MaxValue);
    public static Color Zombie => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(84, 192, 113, byte.MaxValue);

    //Modifiers
    public static Color Blind => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : Color.grey;
    public static Color Hyperfocus => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(0, 60, 95, byte.MaxValue);
    public static Color Unstable => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(57, 255, 20, byte.MaxValue);
    public static Color Ventable => TownOfUsColors.UseBasic ? Palette.CrewmateBlue : new Color32(88, 90, 204, byte.MaxValue);
}
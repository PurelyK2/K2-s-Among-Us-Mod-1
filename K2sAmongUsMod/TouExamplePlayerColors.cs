using MiraAPI.Colors;
using UnityEngine;

namespace TouExtensionExample;

[RegisterCustomColors]
public static class TouExamplePlayerColors
{
    public static CustomColor Spiderman { get; } = new("Spiderman",
        new Color32(255, 0, 0, byte.MaxValue),
        new Color32(0, 0, 255, byte.MaxValue))
    {
        ColorBrightness = CustomColorBrightness.Lighter
    };
    public static CustomColor Sunburn { get; } = new("Sunburn",
        new Color32(255, 255, 80, byte.MaxValue),
        new Color32(241, 42, 47, byte.MaxValue))
    {
        ColorBrightness = CustomColorBrightness.Lighter
    };
}

using K2AmongUs.Modifiers.Game.Universal;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtensionExample.Roles.Crewmate;
using TouExtensionExample.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace TouExtensionExample.Options.Roles.Crewmate;

/// <inheritdoc/>
public sealed class VentableOptions : AbstractOptionGroup<VentableModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Ventable Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Ventable Count", 0f, 15f, 1f)]
    public float VentableCount { get; set; } = 1f;

    /// <inheritdoc/>
    [ModdedNumberOption("Ventable Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float VentableChance { get; set; } = 50f;

    /// <inheritdoc/>
    [ModdedNumberOption("Max Vents", 0f, 100f, 1f)]
    public float MaxVents { get; set; } = 0f;
}
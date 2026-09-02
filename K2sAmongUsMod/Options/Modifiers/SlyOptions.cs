using K2AmongUs.Modifiers.Game.Universal;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using K2AmongUs.Roles.Crewmate;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Crewmate;

/// <inheritdoc/>
public sealed class SlyOptions : AbstractOptionGroup<SlyModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Sly Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Sly Count", 0f, 15f, 1f)]
    public float SlyCount { get; set; } = 1f;

    /// <inheritdoc/>
    [ModdedNumberOption("Sly Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float SlyChance { get; set; } = 50f;
}
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
public sealed class ScrubberOptions : AbstractOptionGroup<ScrubberRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Cleanser Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Scrub Cooldown", 5f, 60f, 5f, MiraNumberSuffixes.Seconds)]
    public float ScrubCooldown { get; set; } = 30f;
    
    /// <inheritdoc/>
    [ModdedNumberOption("Max Scrubs", 0f, 15f, 1f)]
    public float MaxScrubs { get; set; } = 3f;
}
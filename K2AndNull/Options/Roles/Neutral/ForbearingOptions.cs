using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using K2AmongUs.Roles.Crewmate;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Neutral;

/// <inheritdoc/>
public sealed class ForbearingOptions : AbstractOptionGroup<ForbearingRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Forbearing Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Forbearing Kill Cooldown", 5f, 60f, 1f, MiraNumberSuffixes.Seconds)]
    public float RestlessCooldown { get; set; } = 20f;

    /// <inheritdoc/>
    [ModdedToggleOption("Cooldown Decreases Each Tie")]
    public bool RestlessEveryMeeting { get; set; } = false;

    /// <inheritdoc/>
    [ModdedNumberOption("Decrease Per Meeting", 0f, 10f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float RestlessMeetingDecrease { get; set; } = 4f;

    /// <inheritdoc/>
    [ModdedToggleOption("Forbearing Can Vent")]
    public bool ForbearingCanVent { get; set; } = true;
}
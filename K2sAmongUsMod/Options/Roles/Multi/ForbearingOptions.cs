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
public sealed class ForbearingOptions : AbstractOptionGroup<ForbearingRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Forbearing/Restless Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Restless Kill Cooldown", 5f, 60f, 1f, MiraNumberSuffixes.Seconds)]
    public float RestlessCooldown { get; set; } = 20f;

    /// <inheritdoc/>
    [ModdedToggleOption("Cooldown Decreases Each Tie")]
    public bool RestlessEveryMeeting { get; set; } = false;

    /// <inheritdoc/>
    [ModdedNumberOption("Decrease Per Meeting", 0f, 10f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float RestlessMeetingDecrease { get; set; } = 4f;

    /// <inheritdoc/>
    [ModdedToggleOption("Restless Can Vent")]
    public bool RestlessCanVent { get; set; } = true;
}
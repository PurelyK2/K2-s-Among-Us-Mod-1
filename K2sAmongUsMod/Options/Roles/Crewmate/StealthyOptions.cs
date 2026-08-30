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
public sealed class StealthyOptions : AbstractOptionGroup<StealthyRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Snoop Options";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Sneak Cooldown", 0f, 60f, 1f, MiraNumberSuffixes.Seconds)]
    public float SneakCooldown { get; set; } = 30f;

    /// <inheritdoc/>
    [ModdedNumberOption("Sneak Duration", 10f, 120f, 5f, MiraNumberSuffixes.Seconds)]
    public float SneakDuration { get; set; } = 30f;

    /// <inheritdoc/>
    [ModdedNumberOption("Max Sneaks", 0f, 60f, 1f, MiraNumberSuffixes.None, null, true)]
    public float MaxSneaks { get; set; } = 0f;
}
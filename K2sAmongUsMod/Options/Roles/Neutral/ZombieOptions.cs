using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Neutral;

/// <inheritdoc/>
public sealed class ZombieOptions : AbstractOptionGroup<ZombieLeaderRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Zombie Options";

    /// <inheritdoc/>
    [ModdedToggleOption("Zombies Are Revealed")]
    public bool ZombieShowsRole { get; set; } = true;

    /// <inheritdoc/>
    [ModdedNumberOption("Zombie Revive Timer", 0f, 120f, 5f, MiraNumberSuffixes.Seconds)]
    public float ZombieReviveTimer { get; set; } = 5f;
}
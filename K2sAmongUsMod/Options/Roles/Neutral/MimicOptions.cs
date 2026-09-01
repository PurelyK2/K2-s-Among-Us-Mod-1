using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Extensions;

namespace K2AmongUs.Options.Roles.Neutral;

/// <inheritdoc/>
public sealed class MimicOptions : AbstractOptionGroup<MimicRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Mimic Options";

    /// <inheritdoc/>
    [ModdedToggleOption("Mimic Reflects Ability")]
    public bool MimicReflectsAbility { get; set; } = true;

    /// <inheritdoc/>
    [ModdedNumberOption("Mimic Kill Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float KillCooldown { get; set; } = 25f;

    /// <inheritdoc/>
    [ModdedEnumOption("Guess Mimic As", typeof(CacheRoleGuess), new[] {"Mimic", "Mimiced Role", "Mimic or Mimiced Role"})]
    public CacheRoleGuess MimicGuess { get; set; } = CacheRoleGuess.CachedRole;

    /// <inheritdoc/>
    [ModdedToggleOption("Mimic Impostor Vision")]
    public bool ImpostorVision { get; set; } = true;

    /// <inheritdoc/>
    [ModdedToggleOption("Mimic Can Vent")]
    public bool CanVent { get; set; } = true;
}
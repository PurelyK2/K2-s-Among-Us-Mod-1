using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using TownOfUs.Modifiers;
using TownOfUs.Modules;

namespace K2AmongUs.Modifiers.Neutral;

/// <inheritdoc/>
public sealed class ZombieRevealedModifier : BaseRevealModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Zombie Revealed";
    /// <inheritdoc/>
    public override ChangeRoleResult ChangeRoleResult { get; set; } = ChangeRoleResult.Nothing;
    /// <inheritdoc/>
    public override RoleBehaviour ShownRole => Player.GetRoleWhenAlive();

    static bool shouldShow =
        OptionGroupSingleton<ZombieOptions>.Instance.ZombieShowsRole
        || PlayerControl.LocalPlayer.GetRoleWhenAlive() is ZombieLeaderRole
        || PlayerControl.LocalPlayer.GetRoleWhenAlive() is ZombieRole;
    /// <inheritdoc/>
    public override bool RevealRole => shouldShow;
    /// <inheritdoc/>
    public override bool Visible => shouldShow;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}
using K2AmongUs.Options.Roles.Neutral;
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
    /// <inheritdoc/>
    public override bool RevealRole => OptionGroupSingleton<ZombieOptions>.Instance.ZombieShowsRole;
    /// <inheritdoc/>
    public override bool Visible => true;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}
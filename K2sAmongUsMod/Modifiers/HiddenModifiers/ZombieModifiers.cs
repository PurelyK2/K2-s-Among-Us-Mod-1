using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using UnityEngine;

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
    //DestroyableSingleton<RoleManager>.Instance.GetRole(RoleId.Get<ImitatorRole>())
    static bool shouldShow = true;
    
    /// <inheritdoc/>
    public override bool RevealRole => shouldShow;
    /// <inheritdoc/>
    public override bool Visible => shouldShow;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}

/// <inheritdoc/>
public sealed class ZombieArrowModifier(DeadBody deadBody, Color color) : ArrowDeadBodyModifier(deadBody, color, 0)
{
    public override string ModifierName => "Zombie Arrow";
}
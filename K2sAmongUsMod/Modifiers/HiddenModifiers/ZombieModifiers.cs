using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using UnityEngine;
using TownOfUs.Networking;
using MiraAPI.Modifiers;

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
    
    static bool shouldShow = true;
    
    /// <inheritdoc/>
    public override bool RevealRole => shouldShow;
    /// <inheritdoc/>
    public override bool Visible => shouldShow;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}

/// <inheritdoc/>
public sealed class ZombieTransformModifier : DisabledModifier
{
    public bool isZombie = false;

    public override string ModifierName => "Zombie Transform Modifier";
    public override bool CanBeInteractedWith => true;
    public override bool IsConsideredAlive => false;
    public override bool CanUseAbilities => true;
    public override bool CanReport => false;
    public override float Duration => 1f;

    public override void OnDeath(DeathReason reason)
    {
        if(!isZombie)
        {
            Player.RpcFullRevive(false, Player.transform.position, MiraAPI.Roles.RoleId.Get<ZombieRole>());
            Player.RemoveModifier<TownOfUs.Modifiers.Game.Crewmate.TestCleanModifier>();
            Player.AddModifier<ZombieRevealedModifier>();
        }

        isZombie = true;
    }

    public override void OnMeetingStart()
    {
        if(!isZombie)
            ModifierComponent.RemoveModifier(this);
        else
        {
            if(!Player.Data.IsDead)
                Player.RpcSpecialMurder(Player, true, true, true, true, false, false, false, false, "Unalived");
            
            if(!Player.HasModifier<ZombieRevealedModifier>())
                Player.AddModifier<ZombieRevealedModifier>();
        }
    }
}
using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Utilities;
using MiraAPI.Modifiers;
using TownOfUs.Modules;
using MiraAPI.Roles;
using TownOfUs.Networking;
using TownOfUs.Modifiers.Game.Crewmate;
using K2AmongUs.Modifiers.Neutral;
using Reactor.Utilities.Extensions;

namespace K2AmongUs.Buttons.Neutral;

/// <inheritdoc/>
public class ZombieInfectButton : TownOfUsRoleButton<ZombieRole, PlayerControl>
{
    /// <inheritdoc/>
    public override string Name => "INFECT";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public override float Cooldown => 15;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.ReviveSprite;

    public bool LimitedUses => true;
    public override int MaxUses => Role is ZombieRole ? 1 : 2;
    public override MiraAPI.Hud.ButtonUsesMode UsesMode => MiraAPI.Hud.ButtonUsesMode.PerRound;
    
    /// <inheritdoc/>
    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    /// <inheritdoc/>
    public override bool Enabled(RoleBehaviour role)
    {
        return role is ZombieRole || role is ZombieLeaderRole;
    }

    public override void SetOutline(bool active)
    {
    }

    public override bool CanUse()
    {
        return base.CanUse() && !Target.HasModifier<ZombieTransformModifier>();
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        if(Target == null) return;

        if(!Target.HasModifier<ZombieTransformModifier>())
        {
            Target.AddModifier<ZombieTransformModifier>();
        }
    }
}
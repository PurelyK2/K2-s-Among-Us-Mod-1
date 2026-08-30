using MiraAPI.Keybinds;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using UnityEngine;
using MiraAPI.Modifiers;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using Reactor.Utilities.Extensions;
using K2AmongUs.Modifiers.Game.Universal;
using TownOfUs.Buttons;

namespace TouExtensionExample.Buttons.Crewmate;

///  <inheritdoc/>
public sealed class VentableVentButton : TownOfUsTargetButton<Vent>
{
    ///  <inheritdoc/>
    public override string Name => "VENT";
    ///  <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.VentAction;
    ///  <inheritdoc/>
    public override Color TextOutlineColor => Color.red;
    ///  <inheritdoc/>
    public override float Cooldown => 0;
    ///  <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouNeutAssets.JuggVentSprite;

    ///  <inheritdoc/>
    public override bool Enabled(RoleBehaviour? role)
    {
        return !Disabled && role?.Player.HasModifier<VentableModifier>() == true;
    }

    ///  <inheritdoc/>
    public override Vent GetTarget()
    {
        return DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget;
    }

    ///  <inheritdoc/>
    public override void SetOutline(bool active)
    {
        if (Target != null && !PlayerControl.LocalPlayer.HasDied())
        {
            Target.SetOutline(active, true, Color.blue);
        }
    }

    ///  <inheritdoc/>
    public override bool CanUse()
    {
        
        if (TimeLordRewindSystem.IsRewinding)
        {
            return false;
        }
        if (PlayerControl.LocalPlayer.HasDied())
        {
            return false;
        }
        if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
        {
            return false;
        }
        if (PlayerControl.LocalPlayer.GetModifiers<DisabledModifier>(null).Any((DisabledModifier x) => !x.CanUseAbilities))
        {
            return false;
        }
        Vent newTarget = this.GetTarget();
        base.Target = (this.IsTargetValid(newTarget) ? newTarget : null);
        return (PlayerControl.LocalPlayer.inVent || (base.Timer <= 0f && base.Target != null)) && (!base.LimitedUses || base.UsesLeft > 0);
    }

    ///  <inheritdoc/>
    protected override void OnClick()
    {
        if(Target != null && !PlayerControl.LocalPlayer.inVent)
        {
			PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(Target.Id);
			Target.SetButtons(true);
        }
        else if(Target != null)
        {
			PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(Target.Id);
			Target.SetButtons(false);
        }
    }

}
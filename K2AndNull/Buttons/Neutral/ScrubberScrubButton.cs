using K2AmongUs.Assets;
using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Buttons.Crewmate;

///  <inheritdoc/>
public sealed class ScrubberScrubButton : TownOfUsRoleButton<ScrubberRole, PlayerControl>
{
    /// <inheritdoc/>
    public override string Name => "SCRUB";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => K2AndNull.Colors.Scrubber;
    /// <inheritdoc/>
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<ScrubberOptions>.Instance.ScrubCooldown, 5f, 120f);
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => K2RoleIcons.Scrubber;

    /// <inheritdoc/>
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }

    /// <inheritdoc/>
    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }
	
    /// <inheritdoc/>
    public override bool CanUse()
    {
        return base.CanUse() && Target.HasModifier<BaseModifier>();
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Cleanser Cleanse: Target is null");
            return;
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();

        if (Role.Player.AmOwner)
        {
            ScrubberRole.RpcScrubModifiers(Role.Player, Target);
            ResetCooldownAndOrEffect();
        }
    }
    public override float EffectDuration => 5;
}
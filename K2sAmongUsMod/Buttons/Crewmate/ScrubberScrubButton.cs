using K2AmongUs.Modifiers.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
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
    public override Color TextOutlineColor => K2AmongUsColors.Scrubber;
    /// <inheritdoc/>
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<ScrubberOptions>.Instance.ScrubCooldown, 5f, 120f);
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouRoleIcons.Amnesiac;

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
    public override void ClickHandler()
    {
        if (!this.CanClick())
        {
            return;
        }
        this.OnClick();
    }
	
    /// <inheritdoc/>
    public override bool CanUse()
    {
        return base.CanUse() && Target?.GetModifiers<BaseModifier>(null).Any() == true;
    }
    /// <inheritdoc/>
    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Cleanser Cleanse: Target is null");
            return;
        }

        if(Role.Player.AmOwner)
        {
            Target.RpcAddModifier<ScrubberScrubModifier>();
            ResetCooldownAndOrEffect();
        }
    }
}
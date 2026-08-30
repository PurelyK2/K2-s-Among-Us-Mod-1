using System.Runtime.CompilerServices;
using Il2CppMono.Security.Interface;
using K2AmongUs.Modifiers.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using Rewired;
using TouExtensionExample.Assets;
using TouExtensionExample.Options.Roles.Crewmate;
using TouExtensionExample.Roles.Crewmate;
using TouExtensionExample.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Extensions;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace TouExtensionExample.Buttons.Crewmate;

///  <inheritdoc/>
public sealed class ScrubberScrubButton : TownOfUsRoleButton<ScrubberRole, PlayerControl>
{
    /// <inheritdoc/>
    public override string Name => "SCRUB";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => TouExampleColors.Scrubber;
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
    public override ButtonUsesMode UsesMode => ButtonUsesMode.PerGame;
    /// <inheritdoc/>
    public override int MaxUses => (int)OptionGroupSingleton<ScrubberOptions>.Instance.MaxScrubs;

	
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
            SetUses(UsesLeft - 1);
            ResetCooldownAndOrEffect();
            if(UsesLeft == 0 && Button != null)
                Button.usesRemainingSprite.color = Color.grey;
        }
    }
}
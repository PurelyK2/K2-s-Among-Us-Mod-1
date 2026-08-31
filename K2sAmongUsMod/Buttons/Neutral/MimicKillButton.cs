using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs;
using TownOfUs.Assets;
using TownOfUs.Buttons.Neutral;

namespace TouExtensionExample.Buttons.Neutral;

/// <inheritdoc/>
public sealed class MimicKillButton : TownOfUsKillRoleButton<MimicRole, PlayerControl>, IDiseaseableButton,
    IKillButton
{
    /// <inheritdoc/>
    public override string Name => TranslationController.Instance.GetStringWithDefault(StringNames.KillLabel, "Kill");
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => K2AmongUsColors.Mimic;
    /// <inheritdoc/>
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<MimicOptions>.Instance.KillCooldown + MapCooldown, 5f, 120f);
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouNeutAssets.GlitchKillSprite;

    /// <inheritdoc/>
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }

    /// <inheritdoc/>
    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    /// <inheritdoc/>
    public override PlayerControl? GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Mimic Kill: Target is null");
            return;
        }

        PlayerControl.LocalPlayer.RpcCustomMurder(Target);
    }
}
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TouExtensionExample.Assets;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs;

namespace TouExtensionExample.Buttons.Neutral;

public sealed class MimicKillButton : TownOfUsKillRoleButton<MimicRole, PlayerControl>, IDiseaseableButton,
    IKillButton
{
    public override string Name => TranslationController.Instance.GetStringWithDefault(StringNames.KillLabel, "Kill");
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => K2AmongUsColors.Mimic;
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<MimicOptions>.Instance.KillCooldown + MapCooldown, 5f, 120f);
    public override LoadableAsset<Sprite> Sprite => ExampleNeutAssets.SentinelKillSprite;

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(this, false));
    }

    public void SetDiseasedTimer(float multiplier)
    {
        SetTimer(Cooldown * multiplier);
    }

    public override PlayerControl? GetTarget()
    {
        if (!OptionGroupSingleton<LoversOptions>.Instance.LoversKillEachOther && PlayerControl.LocalPlayer.IsLover())
        {
            return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance, false, x => !x.IsLover());
        }

        return PlayerControl.LocalPlayer.GetClosestLivingPlayer(true, Distance);
    }

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
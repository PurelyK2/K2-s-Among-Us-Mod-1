using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using K2AmongUs.Options.Roles.Crewmate;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Buttons.Neutral;

public sealed class RestlessKillButton : TownOfUsKillRoleButton<RestlessRole, PlayerControl>, IDiseaseableButton,
    IKillButton
{
    public override string Name => TranslationController.Instance.GetStringWithDefault(StringNames.KillLabel, "Kill");
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    public override Color TextOutlineColor => K2AmongUsColors.Mimic;
    public override float Cooldown => OptionGroupSingleton<ForbearingOptions>.Instance.RestlessCooldown;
    public override LoadableAsset<Sprite> Sprite => TouAssets.KillSprite;

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
            Error("Restless Kill: Target is null");
            return;
        }

        PlayerControl.LocalPlayer.RpcCustomMurder(Target);
    }
}
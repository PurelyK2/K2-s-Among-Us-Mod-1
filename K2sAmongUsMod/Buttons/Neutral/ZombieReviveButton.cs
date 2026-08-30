using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TouExtensionExample.Assets;
using TouExtensionExample.Options.Roles.Crewmate;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using TouExtensionExample.Roles.Crewmate;
using TouExtensionExample.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Roles.Crewmate;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Alliance;
using MiraAPI.Modifiers;
using TownOfUs.Modules;
using MiraAPI.Roles;
using TownOfUs.Modifiers;
using K2AmongUs.Modifiers.Crewmate;
using TownOfUs.Networking;
using TownOfUs.Modifiers.Game.Crewmate;
using K2AmongUs.Modifiers.Neutral;

namespace TouExtensionExample.Buttons.Neutral;

/// <inheritdoc/>
public class ZombieReviveButton : TownOfUsRoleButton<ZombieRole>
{
    /// <inheritdoc/>
    public override string Name => "REVIVE";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public override float Cooldown => 0;
    /// <inheritdoc/>
    public override bool ZeroIsInfinite { get; set; } = true;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.ReviveSprite;

    /// <inheritdoc/>
    public override bool CanUse()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Count > 0;
    }
    /// <inheritdoc/>
    public override bool CanClick()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Count > 0;
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        List<DeadBody> bodiesInRange = Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Where(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieLeaderRole)).ToList();

        if(bodiesInRange.Count > 0)
        {
            ZombieLeaderReviveButton.SetZombieRole(MiscUtils.PlayerById(bodiesInRange[0].ParentId), bodiesInRange[0]);
        }
    }
}

/// <inheritdoc/>
public sealed class ZombieLeaderReviveButton : TownOfUsRoleButton<ZombieLeaderRole>
{
    /// <inheritdoc/>
    public override string Name => "REVIVE";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public override float Cooldown => 0;
    /// <inheritdoc/>
    public override bool ZeroIsInfinite { get; set; } = true;

    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite
    {
        get
        {
            return TouCrewAssets.ReviveSprite;
        }
    }

    /// <inheritdoc/>
    public override bool CanUse()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Any(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieLeaderRole));
    }
    /// <inheritdoc/>
    public override bool CanClick()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Any(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieLeaderRole));
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        List<DeadBody> bodiesInRange = Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Where(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieLeaderRole)).ToList();

        if(bodiesInRange.Count > 0)
        {
            SetZombieRole(MiscUtils.PlayerById(bodiesInRange[0].ParentId), bodiesInRange[0]);
        }
    }

    /// <inheritdoc/>
    public static void SetZombieRole(PlayerControl player, DeadBody body)
    {
        player.RpcFullRevive(false, player.transform.position, RoleId.Get<ZombieRole>());
        body.ClearBody();
        player.RemoveModifier<TestCleanModifier>();
        player.AddModifier<ZombieRevealedModifier>();
    }
}
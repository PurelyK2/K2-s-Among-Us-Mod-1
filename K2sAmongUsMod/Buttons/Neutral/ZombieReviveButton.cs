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
using TownOfUs.Utilities.Appearances;
using TownOfUs.Roles.Impostor;

namespace K2AmongUs.Buttons.Neutral;

/// <inheritdoc/>
public class ZombieReviveButton : TownOfUsButton
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
    public override bool Enabled(RoleBehaviour? role)
    {
        return role is ZombieRole || role is ZombieLeaderRole;
    }

    /// <inheritdoc/>
    public override bool CanUse()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Any(b => MiscUtils.PlayerById(b.ParentId).Data.Role is not ZombieRole);
    }
    /// <inheritdoc/>
    public override bool CanClick()
    {
        return Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Any(b => MiscUtils.PlayerById(b.ParentId).Data.Role is not ZombieRole);
    }

    /// <inheritdoc/>
    protected override void OnClick()
    {
        List<DeadBody> bodiesInRange = Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.transform.position, ShipStatus.Instance.MaxLightRadius * 0.1f, Helpers.CreateFilter(Constants.NotShipMask)).Where(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieLeaderRole)).ToList();

        if (bodiesInRange.Count > 0)
        {
            SetZombieRole(MiscUtils.PlayerById(bodiesInRange[0].ParentId), bodiesInRange[0]);
        }
    }

    /// <inheritdoc/>
    public static void SetZombieRole(PlayerControl player, DeadBody body)
    {
        if (player.HasModifier<ZombieRevealedModifier>()) return;

        foreach (BaseModifier modifier in player.GetModifiers<BaseModifier>().Where(m => m is not IVisualAppearance))
        {
            player.RpcRemoveModifier(modifier.UniqueId);
        }

        player.RpcFullRevive(false, body.TruePosition, RoleId.Get<ZombieRole>(), true);

        body.ClearBody();
    }
}
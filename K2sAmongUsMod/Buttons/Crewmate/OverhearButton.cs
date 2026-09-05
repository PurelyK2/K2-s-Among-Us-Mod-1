using K2AmongUs.Modifiers.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using K2AmongUs.Options.Roles.Crewmate;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Buttons.Crewmate;

///  <inheritdoc/>
public sealed class OverhearButton : TownOfUsRoleButton<GossipRole, PlayerControl>
{
    /// <inheritdoc/>
    public override string Name => "OVERHEAR";
    /// <inheritdoc/>
    public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
    public override Color TextOutlineColor => Color.black;
    /// <inheritdoc/>
    public override float Cooldown => OptionGroupSingleton<GossipOptions>.Instance.GossipCooldown;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> Sprite => TouModifierIcons.Crewpostor;

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
    protected override void OnClick()
    {
        if (Target == null)
        {
            Error("Gossip Overhear: Target is null");
            return;
        }

        foreach(PlayerControl player in PlayerControl.AllPlayerControls)
        {
            if(player.HasModifier<GossipOverhearModifier>())
            {
                player.RemoveModifier<GossipOverhearModifier>();
            }
        }

        GossipOverhearModifier gossipModifier = new GossipOverhearModifier(GossipOverhearModifier.GenerateGossipRoles(Target));

        Target.AddModifier(gossipModifier);

        string notifyString = "You are overhearing someone's conversation.\nYou will tell everyone something about them next meeting.";
        MiraAPI.Utilities.Helpers.CreateAndShowNotification(notifyString, Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Crewpostor.LoadAsset());
    }
}
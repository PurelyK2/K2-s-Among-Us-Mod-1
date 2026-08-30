using System;
using System.Runtime.CompilerServices;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Localization;
using TownOfUs.Options;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Patches;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace K2AmongUs.Modifiers.Crewmate;
    
/// <inheritdoc/>
public sealed class MimicRoleModifier : AllianceGameModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Mimic";
    /// <inheritdoc/>
    public override bool HideOnUi => false;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return 0;
    }
    
    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        ReMimic();
    }

    /// <inheritdoc/>
    public override void OnDeath(DeathReason reason)
    {
        ReMimic();
    }

    void ReMimic()
    {
        Player.RpcChangeRole(RoleId.Get<MimicRole>(), false);
    }

}
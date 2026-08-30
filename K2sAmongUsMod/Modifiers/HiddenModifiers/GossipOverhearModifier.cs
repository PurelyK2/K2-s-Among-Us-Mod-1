using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Rewired;
using TouExtensionExample.Roles.Crewmate;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers.Crewmate;

/// <inheritdoc/>
public sealed class GossipOverhearModifier : BaseModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Gossip Target";
    /// <inheritdoc/>
    public override bool HideOnUi => true;

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is GossipRole)) return;

        if(Player == null)
        {
            Error("Player Is Null For Gossip");
            return;
        }        
        
        GossipRole.GenerateGossip(Player);
        Player.RemoveModifier(this);
    }

}
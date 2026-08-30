using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using K2AmongUs.Modifiers.Game.Universal;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules;
using TownOfUs.Options.Maps;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using Il2CppSystem.Xml;
using K2AmongUs.Modifiers.Game.Alliance;
using Reactor.Utilities.Extensions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers.Types;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Game.Assailant;
using Il2CppSystem.Web.Util;
using K2AmongUs.Modifiers.Crewmate;
using K2AmongUs.Modifiers.Neutral;
using TownOfUs.Networking;
using MiraAPI.Events.Vanilla.Meeting;

namespace K2AmongUs.Patches;

/// <inheritdoc/>
public static class ZombiePatches
{
    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void HandleVoteEvent(MiraAPI.Events.Vanilla.Meeting.Voting.HandleVoteEvent @event)
    {
        ZombieLeaderRole? leader = @event.VoteData.Owner.Data.Role as ZombieLeaderRole;
        if (leader != null)
        {
            @event.VoteData.SetRemainingVotes(0);
            for (int i = 0; i < PlayerControl.AllPlayerControls.ToArray().Count(p => p.GetRoleWhenAlive() is ZombieRole) + 1; i++)
            {
                @event.VoteData.VoteForPlayer(@event.TargetId);
            }
            @event.Cancel();
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
	public static void OnRoundStart(RoundStartEvent @event)
    {
        if(!PlayerControl.AllPlayerControls.ToArray().Any(p => p.GetRoleWhenAlive() is ZombieLeaderRole)) return;
        
        foreach(PlayerControl player in PlayerControl.AllPlayerControls)
        {
            if(player.GetRoleWhenAlive() is ZombieRole)
            {
                player.RpcBasicRevive();
            }
        }
    }
}
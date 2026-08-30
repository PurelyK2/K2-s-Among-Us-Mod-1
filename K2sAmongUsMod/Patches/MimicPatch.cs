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

namespace K2AmongUs.Patches;

/// <inheritdoc/>
public static class MimicPatches
{
    //Disable Assassin For Mimic
    /// <inheritdoc/>
    [HarmonyPatch(typeof(ModifierManager), "IsGameModifierValidOn", new Type[] { typeof(PlayerControl), typeof(GameModifier), typeof(uint) })]
    public static class GameModifierValidityPatch
    {
        /// <inheritdoc/>
        public static void Postifx(ref PlayerControl player, ref uint modifierId, ref bool __result)
        {
            if(player.GetRoleWhenAlive() is MimicRole && modifierId == ModifierManager.GetModifierTypeId(typeof(AssassinModifier)))
            {
                __result = false;
            }
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void OnRoundStart(RoundStartEvent @event)
    {
        if(!PlayerControl.LocalPlayer.HasModifier<MimicRoleModifier>()) return;

        PlayerControl.LocalPlayer.RpcChangeRole(RoleId.Get<MimicRole>());

        if(MiraAPI.Utilities.Helpers.GetAlivePlayers().Count < 2) return;

        //open menu
        foreach(MimicRole? mimic in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(x => x.GetRoleWhenAlive() is MimicRole).Select(x => x.GetRoleWhenAlive() as MimicRole))
        {
            mimic?.OpenPickingUI();
        }
    }
}
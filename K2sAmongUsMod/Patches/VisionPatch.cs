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

namespace TouExtensionExample.Patches;

/// <inheritdoc/>
[HarmonyPriority(Priority.Last)]
[HarmonyPatch(typeof(ShipStatus), "CalculateLightRadius")]
public static class VisionPatch
{
/// <inheritdoc/>
    public static void Postfix(ShipStatus __instance, NetworkedPlayerInfo player, ref float __result)
    {
        if (MiscUtils.CurrentGamemode() == TouGamemode.HideAndSeek)
        { 
            return;
        }
        if (player == null || player.IsDead)
        {
            __result = __instance.MaxLightRadius;
            return;
        }

        if(player.Object.HasModifier<BlindModifier>(null))
        {
            __result *= (100f - (float)OptionGroupSingleton<BlindOptions>.Instance.BlindAmount) / 100f;
        }

        if(player.Object.HasModifier<HyperfocusModifier>() && Minigame.Instance != null)
        {
            __result *= 0f;
        }
    }
}
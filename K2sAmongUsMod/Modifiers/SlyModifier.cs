using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options.Roles.Crewmate;
using TownOfUs.Modifiers;
using MiraAPI.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using HarmonyLib;
using TownOfUs.Roles.Crewmate;
using MiraAPI.Roles;
using TownOfUs.Roles.Neutral;
using Il2CppSystem.Web.Util;
using AmongUs.GameOptions;
using TownOfUs.Roles;
using TownOfUs.Buttons.Crewmate;

namespace K2AmongUs.Modifiers.Game.Universal;

/// <inheritdoc/>
public sealed class SlyModifier : TouGameModifier, IWikiDiscoverable
{
    /// <inheritdoc/>
    public override string ModifierName => "Sly";
    /// <inheritdoc/>
    public override string LocaleKey => "Sly";
    
    /// <inheritdoc/>
    public override string IntroInfo => "You are definitely not bad...";

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return IntroInfo;
    }
    /// <inheritdoc/>
    public string GetAdvancedDescription()
    {
        return GetDescription() + MiscUtils.AppendOptionsText(base.GetType());
    }
    /// <inheritdoc/>
    public override ModifierFaction FactionType => ModifierFaction.UniversalUtility;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<SlyOptions>.Instance.SlyChance;
    }

    /// <inheritdoc/>
    public override float IntroSize => 3f;
    /// <inheritdoc/>
    public override bool HideOnUi => false;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> ModifierIcon => TouModifierIcons.Scout;
    /// <inheritdoc/>
    public override int GetAmountPerGame()
    {
        return CustomAmount;
    }
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<SlyOptions>.Instance.SlyCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<SlyOptions>.Instance.SlyChance;

    /// <inheritdoc/>
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return (!role.IsCrewmate() || role.Player.HasModifier<AllianceGameModifier>())
            && !(role is JesterRole);
    }


    /// =========================== PATCHES ===========================
    
    /// <inheritdoc/>
    [HarmonyPatch(typeof(SeerRevealButton), "IsEvil", [typeof(PlayerControl)])]
    public static class OnSeerCheckEvil
    {
        /// <inheritdoc/>
        public static void Postfix(ref bool __result, PlayerControl target)
        {
            if(target.HasModifier<SlyModifier>())
                __result = false;
        }
    }

    /// <inheritdoc/>
    [HarmonyPatch(typeof(TrapperRole), "Report")]
    public static class OnTrapperReport
    {
        /// <inheritdoc/>
        public static void Prefix(ref TrapperRole __instance)
        {
            for(int i = 0; i < __instance.TrappedPlayers.Count; i++)
            {
                PlayerControl? trappedPlayer = __instance.TrappedPlayers[i].Player;

                if(trappedPlayer == null)
                {
                    Error("Trapped Player Doesn't Have An Attached PlayerControl");
                    continue;
                }

                if(trappedPlayer.HasModifier<SlyModifier>())
                {
                    __instance.TrappedPlayers[i] = DestroyableSingleton<RoleManager>.Instance.GetRole(0);
                }
            }
        }
    }
    
    /// <inheritdoc/>
    [HarmonyPatch(typeof(LookoutWatchedModifier), "OnMeetingStart")]
    public static class OnLookoutWatch
    {
        /// <inheritdoc/>
        public static void Prefix(ref LookoutWatchedModifier __instance)
        {
            foreach (KeyValuePair<PlayerControl, RoleBehaviour> kvp in __instance.SeenPlayers)
            {
                if(kvp.Value.Player.HasModifier<SlyModifier>())
                {
                    __instance.SeenPlayers[kvp.Key] = DestroyableSingleton<RoleManager>.Instance.GetRole(0);
                }
            }
        }
    }
}
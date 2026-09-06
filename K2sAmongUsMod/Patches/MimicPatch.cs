using HarmonyLib;
using K2AmongUs.Modifiers.Crewmate;
using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Game.Assailant;
using TownOfUs.Modules;
using TownOfUs.Utilities;

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
}
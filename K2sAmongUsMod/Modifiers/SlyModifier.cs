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
    public override void Update()
    {
        base.Update();

        if(Player.HasModifier<SeerEvilRevealModifier>())
        {
            Player.RemoveModifier<SeerEvilRevealModifier>();
            Player.AddModifier<SeerGoodRevealModifier>();
        }
    }
        /// <inheritdoc/>
    [HarmonyPatch(typeof(TrapperRole), "Report")]
    public static class OnTrapperRoleReport
    {
        /// <inheritdoc/>
        public static void Prefix(TrapperRole __instance)
        {
            List<RoleBehaviour> trappedPlayers = new List<RoleBehaviour>();
            foreach(RoleBehaviour role in __instance.TrappedPlayers)
            {
                if(role.Player.HasModifier<SlyModifier>())
                {
                    trappedPlayers.Add((RoleBehaviour)RoleId.Get<SurvivorRole>());
                }
                else
                {
                    trappedPlayers.Add(role);
                }
            }

            __instance.TrappedPlayers = trappedPlayers;
        }
    }
}
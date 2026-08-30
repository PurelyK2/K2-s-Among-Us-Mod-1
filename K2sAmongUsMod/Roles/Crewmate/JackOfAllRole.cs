using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using K2AmongUs.Modifiers.Crewmate;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Rewired;
using TouExtensionExample.Assets;
using TouExtensionExample.Buttons.Crewmate;
using TouExtensionExample.Options.Roles.Crewmate;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Game.Assailant;
using TownOfUs.Modifiers.Game.Impostor;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Roles.Other;
using TownOfUs.Utilities;
using UnityEngine;

namespace TouExtensionExample.Roles.Crewmate;

/// <inheritdoc/>
public sealed class JackOfAllRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Perception;
    /// <inheritdoc/>
    public string LocaleKey => "Jack-Of-All-Trades";
    /// <inheritdoc/>
    public string RoleName => "Jack-Of-All-Trades";
    /// <inheritdoc/>
    public string RoleDescription => "Have a lot of modifiers";
    /// <inheritdoc/>
    public string RoleLongDescription => RoleDescription + "\n(May Get More By Doing Tasks)";

    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;

    /// <inheritdoc/>
    public Color RoleColor => Color.white;

    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.DetectiveIntroSound,
        Icon = TouRoleIcons.Amnesiac
    };


    /// <inheritdoc/>
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }

    /// <inheritdoc/>
    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);

        if(player.AmOwner)
        {
            try
            {
                GiveRandModifiers(5 - player.GetModifiers<BaseModifier>().Count(), player); //Modifiers count from options
            }
            catch(System.Exception e)
            {
                Fatal(e);
            }
        }
    }
    static void GiveRandModifiers(int count, PlayerControl player)
    {
        if(count < 1) return;

        try
        {
            List<BaseModifier> modifiers =
                    ModifierManager.Modifiers
                    .Where(m => m is GameModifier
                    && (m as GameModifier)?.GetAmountPerGame() > 0
                    && (m as GameModifier)?.GetAssignmentChance() > 0
                    && (m as GameModifier)?.CanSpawnOnCurrentMode() == true
                    && !player.HasModifier(m.TypeId)
                    && !(m is DeadlyQuotaModifier)
                ).ToList();

            if(modifiers.Count == 0)
            {
			    Error("No modifiers to give");
                return;
            }

            for(int i = 0; i < count; i++)
            {
                BaseModifier modifier;
                do
                {
                    modifier = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                    modifiers.Remove(modifier);
                } while (player.HasModifier(modifier.TypeId));
                
                Error("Added " + modifier.ModifierName + " to JOAR");

                player.RpcAddModifier(modifier.TypeId, Array.Empty<object>());
            }
        }
        catch(System.Exception e)
        {
			Fatal(e);
        }
    }

    /// <inheritdoc/>
    public static void CheckAddModifier(PlayerControl player)
    {
        if(player.AmOwner && player.Data.Role is JackOfAllRole)
        {
			Info("Completed Task");
            GiveRandModifiers(1, player);
        }
    }
}
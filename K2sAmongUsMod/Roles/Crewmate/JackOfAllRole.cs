using K2AmongUs.Options.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Game.Impostor;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Crewmate;

/// <inheritdoc/>
public sealed class JackOfAllRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public int NumTasksUntilMod = (int)OptionGroupSingleton<JackOfAllOptions>.Instance.TasksPerMod;

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
        Icon = TouRoleIcons.Agent
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
                GiveRandModifiers((int)OptionGroupSingleton<JackOfAllOptions>.Instance.NumModifiers - player.GetModifiers<BaseModifier>().Count(), player);
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
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("There Are No Modifiers Left To Give", Color.yellow, new Vector3(0f, 1f, -20f), null, null);

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
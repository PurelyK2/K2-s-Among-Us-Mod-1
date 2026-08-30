using System.Collections;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Patches.WinConditions;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Networking;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public class ZombieRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IUnguessable
{
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    /// <inheritdoc/>
    public string RoleName => "Zombie";
    /// <inheritdoc/>
    public string RoleDescription => "THE APOCOLYPSE HAS BEGUN!";
    /// <inheritdoc/>
    public string RoleLongDescription => "Convert Dead Players Into Zombies.";
    /// <inheritdoc/>
    public Color RoleColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    /// <inheritdoc/>
    public RoleBehaviour AppearAs => (RoleBehaviour)RoleId.Get<ZombieRole>();
    /// <inheritdoc/>
    public bool IsGuessable => false;
    /// <inheritdoc/>
    public new bool IsDraftable => false;
    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Haunter,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        DefaultRoleCount = 0,
        MaxRoleCount = 0,
    };


    /// <inheritdoc/>
    public override void OnRoleSet()
    {
        Player.AddModifier<ZombieRevealedModifier>();
    }

    /// <inheritdoc/>
    public override bool DidWin(GameOverReason gameOverReason)
    {
        if(Helpers.GetAlivePlayers().FirstOrDefault(p => p.GetRoleWhenAlive() is ZombieLeaderRole)?.GetRoleWhenAlive() is ZombieLeaderRole zombieLeader)
        {
            return zombieLeader.DidWin(gameOverReason);
        }
        return false;
    }

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        Player.RpcSpecialMurder(Player, true, true, true, true, false, false, false, false, "Unalived");
        
        if(!Player.HasModifier<ZombieRevealedModifier>())
            Player.AddModifier<ZombieRevealedModifier>();
    }

    /// <inheritdoc/>
    public override void OnVotingComplete()
    {
        if(Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is ZombieLeaderRole))
            Player.RpcBasicRevive();
    }

}

/// <inheritdoc/>
public sealed class ZombieLeaderRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnguessable
{
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Death;
    /// <inheritdoc/>
    public string RoleName => "Zombie Leader";
    /// <inheritdoc/>
    public string RoleDescription => "START AN APOCOLYPSE";
    /// <inheritdoc/>
    public string RoleLongDescription => "Convert Dead Players Into Zombies.";
    /// <inheritdoc/>
    public Color RoleColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    /// <inheritdoc/>
    public RoleBehaviour AppearAs => (RoleBehaviour)RoleId.Get<ZombieLeaderRole>();
    /// <inheritdoc/>
    public bool IsGuessable => false;
    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.ScreamIntro,
        Icon = TouRoleIcons.Haunter
    };

    float timer;
    /// <inheritdoc/>
    public void Update()
    {
        if(Player == null || Player.Data.IsDead) return;

        List<DeadBody> bodiesInRange = Helpers.GetNearestDeadBodies(Player.transform.position, ShipStatus.Instance.MaxLightRadius * 100, Helpers.CreateFilter(Constants.NotShipMask));
        bodiesInRange.RemoveAll(b => !(MiscUtils.PlayerById(b.ParentId).GetRoleWhenAlive() is ZombieRole));

        if(bodiesInRange.Count > 0)
        {
            if(timer <= 0)
            {
                PlayerControl player = MiscUtils.PlayerById(bodiesInRange[0].ParentId);

                player.RpcBasicRevive();
                bodiesInRange[0].ClearBody();
                timer = 5;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            timer = 5;
        }

        if(!Helpers.GetAlivePlayers().Any(p => !(p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole)))
        {
            Info("Should Win!");
            NetworkedPlayerInfo[] winners = PlayerControl.AllPlayerControls.ToArray().Where(p => !(p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole)).Select(p => p.Data).ToArray();
		    CustomGameOver.Trigger<ZombieGameOver>(winners);
        }
    }

    /// <inheritdoc/>
    public override void OnDeath(DeathReason reason)
    {
        List<PlayerControl> zombies = Helpers.GetAlivePlayers().Where(p => p.GetRoleWhenAlive() is ZombieRole).ToList();
        
        Player.RpcSpecialMultiMurder(zombies, true, true, true, true, true, false, false, false, "Leaderless");

        foreach(PlayerControl zombie in zombies)
        {
            DeathHandlerModifier? modifier = zombie.GetModifier<DeathHandlerModifier>();
            if(modifier != null)
            {
                modifier.CauseOfDeath = "Leaderless";
                modifier.ExtendedCauseOfDeath = "Leaderless";
            }
            else
            {
                Error("Zombie Doesn't Have Death Handler Modifier");
            }
        }
    }

    /// <inheritdoc/>
    public override bool DidWin(GameOverReason gameOverReason)
    {
        if (Player == null)
        {
            return false;
        }
        else
        {
            return !Helpers.GetAlivePlayers().Any(p => !(p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole));
        }
    }
}
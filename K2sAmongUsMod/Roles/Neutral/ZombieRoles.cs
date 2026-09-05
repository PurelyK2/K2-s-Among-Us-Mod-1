using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Patches.WinConditions;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Networking;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Utilities;
using TownOfUs.Events.TouEvents;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public class ZombieRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IUnguessable, IContinuesGame
{
    /// <inheritdoc/>
    public bool HasImpostorVision => true;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    /// <inheritdoc/>
    public string RoleName => "Zombie";
    /// <inheritdoc/>
    public string RoleDescription => "THE APOCOLYPSE HAS BEGUN!";
    /// <inheritdoc/>
    public string RoleLongDescription => "Convert Dead Players Into Zombies.";
    
    /// <inheritdoc/>
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }

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
        Icon = TouRoleIcons.Altruist,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        DefaultRoleCount = 0,
        MaxRoleCount = 0,
        TasksCountForProgress = false,
    };


    /// <inheritdoc/>
    public override void OnRoleSet()
    {
        foreach(BaseModifier modifier in Player.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi))
        {
            Player.RemoveModifier(modifier);
        }
        Player.AddModifier<ZombieRevealedModifier>();
    }
    
    /// <inheritdoc/>
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<AltruistRole>();

    /// <inheritdoc/>
    public bool ContinuesGame =>
        PlayerControl.AllPlayerControls.ToArray().Any(p => p.GetRoleWhenAlive() is ZombieRole)
        || MiraAPI.Utilities.Helpers.GetNearestDeadBodies(PlayerControl.LocalPlayer.GetTruePosition(), 100000, Helpers.CreateFilter(Constants.NotShipMask)).Count > 0;

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
    
    /// <inheritdoc/>
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>()!;
        return console == null || console.AllowImpostor;
    }

    /// <inheritdoc/>
    public void Update()
    {
        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is ZombieLeaderRole))
        {
            Player.RpcSpecialMurder(Player, true, true, true, false, false, false, true, true, "Leaderless");
            Player.RpcChangeRole(RoleId.Get<NeutralGhostRole>());
        }
    }
}

/// <inheritdoc/>
public sealed class ZombieLeaderRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnguessable, ICrewVariant, IContinuesGame
{
    /// <inheritdoc/>
    public bool HasImpostorVision => true;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Death;
    /// <inheritdoc/>
    public string RoleName => "Zombie Leader";
    /// <inheritdoc/>
    public string RoleDescription => "START AN APOCOLYPSE";
    /// <inheritdoc/>
    public string RoleLongDescription => "Convert Dead Players Into Zombies. To win alone!";
    
    /// <inheritdoc/>
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }

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
        Icon = TouNeutAssets.PestKillSprite,
    };

    /// <inheritdoc/>
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<AltruistRole>();


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
                timer = OptionGroupSingleton<ZombieOptions>.Instance.ZombieReviveTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            timer = OptionGroupSingleton<ZombieOptions>.Instance.ZombieReviveTimer;
        }

        if(!Helpers.GetAlivePlayers().Any(p => !(p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole)))
        {
            Info("Should Win!");
            NetworkedPlayerInfo[] winners = PlayerControl.AllPlayerControls.ToArray().Where(p => p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole).Select(p => p.Data).ToArray();
		    CustomGameOver.Trigger<ZombieGameOver>(winners);
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
    
    /// <inheritdoc/>
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>()!;
        return console == null || console.AllowImpostor;
    }

    // Zombie Leader Arrow
    [RegisterEvent(0)]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        if (!CustomRoleUtils.GetActiveRolesOfType<ZombieLeaderRole>().HasAny())
        {
            return;
        }

        if (!OptionGroupSingleton<ZombieOptions>.Instance.ZombieArrows)
        {
            return;
        }

        Coroutines.Start(CoCreateArrow(@event.Target));
    }

    private static System.Collections.IEnumerator CoCreateArrow(PlayerControl target)
    {
        var deadBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == target.PlayerId);

        if (deadBody == null)
        {
            yield break;
        }

        foreach (var zombieRole in CustomRoleUtils.GetActiveRolesOfType<ZombieLeaderRole>().Select(x => x.Player))
        {
            if (zombieRole.AmOwner)
            {
                zombieRole.AddModifier<ZombieArrowModifier>(deadBody, Color.white);
            }
        }
    }

    public bool ContinuesGame
    {
        get
        {
            bool killersAlive = TownOfUs.Utilities.MiscUtils.KillersAliveCount > 0;
            bool hasZombies = PlayerControl.AllPlayerControls.ToArray().Any(p => p.Data.Role is ZombieRole);

            return (killersAlive && MiraAPI.Utilities.Helpers.GetAlivePlayers().Count >= 3) || hasZombies;
        }
    }
}
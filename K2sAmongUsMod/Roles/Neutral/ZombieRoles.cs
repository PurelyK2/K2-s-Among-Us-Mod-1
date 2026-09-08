
using K2AmongUs.Assets;
using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Patches.WinConditions;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TownOfUs.Assets;
using TownOfUs.Events;
using TownOfUs.Events.TouEvents;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Networking;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Patches;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using TownOfUs.Utilities.Appearances;
using UnityEngine;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public class ZombieRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IUnguessable, IGhostRole
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
        Icon = K2RoleIcons.Zombie,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        DefaultRoleCount = 0,
        MaxRoleCount = 0,
        TasksCountForProgress = false,
    };

    /// <inheritdoc/>
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
				new("Infect", "Mark A Player. If They Die This Round, They Will Revive As A Zombie", TouRoleIcons.Altruist),
            };
        }
    }

    /// <inheritdoc/>
    public override void OnRoleSet()
    {
        foreach(BaseModifier modifier in Player.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi))
        {
            Player.RemoveModifier(modifier);
        }
        Player.AddModifier<ZombieRevealedModifier>();
        Player.AddModifier<ZombieAllianceModifier>();
    }

    public bool WinConditionMet()
    {
        if (Helpers.GetAlivePlayers().FirstOrDefault(p => p.GetRoleWhenAlive() is ZombieLeaderRole)?.GetRoleWhenAlive() is ZombieLeaderRole zombieLeader)
        {
            return zombieLeader.WinConditionMet();
        }
        return false;
    }

    /// <inheritdoc/>
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
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
        if(Player == null || Player.Data.IsDead) return;

        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is ZombieLeaderRole))
        {
            Player.RpcSpecialMurder(Player, true, true, true, false, false, false, true, true, "Leaderless");
            Player.RpcChangeRole(RoleId.Get<NeutralGhostRole>());
        }
    }

    #region Ghost Role Stuff
    public bool Setup { get; set; }
    public bool Caught { get; set; }
    public bool Faded { get; set; }
    public bool CanBeClicked { get; set; } = false;
    public void Spawn()
    {
        this.Setup = true;
        bool camouflageCommsEnabled = HudManagerPatches.CamouflageCommsEnabled;
        if (camouflageCommsEnabled)
        {
            base.Player.SetCamouflage(false);
        }
        string text = "Setup HaunterRole '" + base.Player.Data.PlayerName + "'";

        MiscUtils.LogInfo(TownOfUsEventHandlers.LogLevel.Error, text);
        base.Player.gameObject.layer = LayerMask.NameToLayer("Players");
        base.Player.gameObject.GetComponent<PassiveButton>().OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        base.Player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        bool amOwner = base.Player.AmOwner;
        if (amOwner)
        {
            base.Player.SpawnAtRandomVent();
            base.Player.MyPhysics.ResetMoveState(true);
            DestroyableSingleton<HudManager>.Instance.SetHudActive(false);
            DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
            DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
            HudManagerPatches.ResetZoom();
        }
    }
    public void FadeUpdate() { }
    public void Clicked() { }
    public bool CanCatch() { return false; }
    #endregion
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
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
				new("Infect", "Mark A Player. If They Die This Round, They Will Revive As A Zombie", TouRoleIcons.Altruist),
            };
        }
    }

    /// <inheritdoc/>
    public Color RoleColor => new Color32(84, 192, 113, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    /// <inheritdoc/>
    public RoleBehaviour AppearAs => (RoleBehaviour)RoleId.Get<ZombieLeaderRole>();
    /// <inheritdoc/>
    public bool IsGuessable => PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role is VigilanteRole;
    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.ScreamIntro,
        Icon = K2RoleIcons.ZombieLeader,
    };

    /// <inheritdoc/>
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<AltruistRole>();

    public override void OnRoleSet()
    {
        base.OnRoleSet();

        Player.AddModifier<ZombieAllianceModifier>();
    }
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

                player.RpcFullRevive(true, bodiesInRange[0].TruePosition, RoleId.Get<ZombieRole>(), true);
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

        if(!Player.Data.IsDead)
        {
            int numNonZombies = Helpers.GetAlivePlayers().Count(p => p.Data.Role is not ZombieRole && p.Data.Role is not ZombieLeaderRole);
            IEnumerable<NetworkedPlayerInfo> zombies = PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole || p.Data.Role is ZombieLeaderRole).Select(p => p.Data);

            if(numNonZombies < zombies.Count() && MiscUtils.KillersAliveCount == 0)
            {
                Info("Zombies Should Win!");
                CustomGameOver.Trigger<ZombieGameOver>(zombies);
            }
        }
    }

    public bool WinConditionMet()
    {
        if (Player.Data.IsDead) return false;

        int numNonZombies = Helpers.GetAlivePlayers().Count(p => p.Data.Role is not ZombieRole && p.Data.Role is not ZombieLeaderRole);
        IEnumerable<NetworkedPlayerInfo> zombies = PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole || p.Data.Role is ZombieLeaderRole).Select(p => p.Data);

        return numNonZombies < zombies.Count() && MiscUtils.KillersAliveCount == 0;
    }

    /// <inheritdoc/>
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
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

    public bool ContinuesGame
    {
        get
        {
            bool killersAlive = TownOfUs.Utilities.MiscUtils.KillersAliveCount > 0;
            bool hasZombies = PlayerControl.AllPlayerControls.ToArray().Any(p => p.Data.Role is ZombieRole);
            bool canGetDeadBody = Helpers.GetNearestDeadBodies(Player.transform.position, ShipStatus.Instance.MaxLightRadius * 100, Helpers.CreateFilter(Constants.NotShipMask)).Count > 0;

            return (killersAlive && MiraAPI.Utilities.Helpers.GetAlivePlayers().Count >= 3) || hasZombies || canGetDeadBody;
        }
    }
}
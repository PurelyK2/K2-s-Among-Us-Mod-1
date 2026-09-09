using Il2CppInterop.Runtime.Attributes;
using K2AmongUs.Modifiers.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Components;
using TownOfUs.Modules.Wiki;
using TownOfUs.Networking;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public sealed class ScrubberRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public bool didWin { get; set; }

    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    /// <inheritdoc/>
    public string LocaleKey => "Scrubber";
    /// <inheritdoc/>
    public string RoleName => "Scrubber";
    /// <inheritdoc/>
    public string RoleDescription => "Cleanse the land of modifiers to win";
    /// <inheritdoc/>
    public string RoleLongDescription => RoleDescription;

    /// <inheritdoc/>
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }

    /// <inheritdoc/>
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
				new("Cleanse", "Cleanse A Player Of All Their Modifiers", TouRoleIcons.Amnesiac),
            };
        }
    }

    /// <inheritdoc/>
    public Color RoleColor => new Color32(230, 242, 200, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralOutlier;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.JanitorCleanSound,
        Icon = TouRoleIcons.Infestor
    };

    public void Update()
    {
        if (Player == null || Player.Data.IsDead || didWin || !Player.AmOwner) return;

        didWin = !MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => !p.HasModifier<ScrubberScrubModifier>() && p.Data.Role is not ScrubberRole);

        if(didWin)
        {
            Info("Scrubber Should Win");
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("The world has been cleansed of impurities, the only thing left to cleanse is yourself...", Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Bait.LoadAsset());

            foreach(PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.HasModifier<ScrubberScrubModifier>()))
            {
                player.RemoveModifier<ScrubberScrubModifier>();
            }
        }
    }

    public void OnRoundStart()
    {
        if(Player.AmOwner && didWin)
        {
            /*
            PlayerStats stats = GameHistory.PlayerStats[Player.PlayerId];
            stats.DeathString =  "Cleansed";
            stats.DiedThisRound = false;
            stats.PlayerState = StoredPlayerState.Dead;
            stats.LockDeathInfo = true;
            */
            Player.Exiled();
        }
    }

    public bool CheckDidWin()
    {
        if (didWin) return true;

        didWin = !MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => !p.HasModifier<ScrubberScrubModifier>() && p.Data.Role is not ScrubberRole);
        
        return didWin;
    }

    public bool GetDidWin()
    {
        return didWin;
    }
    public bool WinConditionMet()
    {
        return didWin;
    }

    public bool MetWinCon => didWin;

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return this.didWin;
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



    [RegisterEvent(0)]
    public static void RoundStartHandler(RoundStartEvent @event)
    {
        if(@event.TriggeredByIntro) return;

        if(PlayerControl.LocalPlayer.Data.Role is ScrubberRole scrubber)
            scrubber.OnRoundStart();
    }
}
using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using K2AmongUs.Options.Roles.Crewmate;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Roles.Crewmate;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public sealed class ForbearingRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Trickster;
    /// <inheritdoc/>
    public string LocaleKey => "Forbearing";
    /// <inheritdoc/>
    public string RoleName => "Forbearing";
    /// <inheritdoc/>
    public string RoleDescription => "You are patient, but irritated...";
    /// <inheritdoc/>
    public string RoleLongDescription => RoleDescription + "\n(If a meeting ties, become a neutral killer)";

    /// <inheritdoc/>
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }
    
    /// <inheritdoc/>
    public Color RoleColor => new Color32(217, 84, 77, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.SinisterIntro,
        Icon = TouRoleIcons.Jackal,
        TasksCountForProgress = true
    };

    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<SheriffRole>();

    /// <inheritdoc/>
    public override void OnVotingComplete()
    {
        if(MeetingHud.Instance.exiledPlayer == null)
        {
            if(PlayerControl.LocalPlayer.GetRoleWhenAlive() is ForbearingRole)
                Player.RpcChangeRole(RoleId.Get<RestlessRole>());

            MiraAPI.Utilities.Helpers.CreateAndShowNotification(
                "There is no decision, a killer has awoken...",
                RoleColor,
                new Vector3(0f, 1f, -20f),
                null,
                TouModifierIcons.Egotist.LoadAsset()
            );
        }
    }

    /// <inheritdoc/>
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return  DestroyableSingleton<RoleManager>.Instance.GetRole(0).DidWin(gameOverReason);
    }
}

/// <inheritdoc/>
public sealed class RestlessRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, IUnguessable
{
    /// <inheritdoc/>
    public int numMeetingsSkipped;

    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Fearmonger;

    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;

    /// <inheritdoc/>
    public bool HasImpostorVision { get { return true; } }
    /// <inheritdoc/>
    public string RoleName => "Restless";

    /// <inheritdoc/>
    public string RoleDescription => "You have become impatient, the time to kill has come";

    /// <inheritdoc/>
    public string RoleLongDescription => "(Comes From Forbearing) Once a meeting is tied, you'll turn into this role";

    /// <inheritdoc/>
    public Color RoleColor => new Color32(217, 84, 77, byte.MaxValue);

    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;

    /// <inheritdoc/>
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<SheriffRole>();

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouRoleIcons.Jackal,
        HideSettings = true,
        CanModifyChance = false,
        DefaultChance = 0,
        MaxRoleCount = 0,
        CanUseVent = OptionGroupSingleton<ForbearingOptions>.Instance.RestlessCanVent,
        TasksCountForProgress = false
    };

    /// <inheritdoc/>
    public override void OnVotingComplete()
    {
        if(MeetingHud.Instance.exiledPlayer == null)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification(
                "The killer is getting tired of waiting...",
                RoleColor,
                new Vector3(0f, 1f, -20f),
                null,
                TouRoleIcons.Jackal.LoadAsset()
            );

            if(OptionGroupSingleton<ForbearingOptions>.Instance.RestlessEveryMeeting)
            {
                numMeetingsSkipped++;
            }
        }
    }

    /// <inheritdoc/>
    public new bool IsDraftable => false;
    /// <inheritdoc/>
    public RoleBehaviour AppearAs => DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<ForbearingRole>());
    /// <inheritdoc/>
    public bool IsGuessable => false;

    /// <inheritdoc/>
    public bool WinConditionMet()
    {
        return !Player.HasDied() && Helpers.GetAlivePlayers().Count <= 2 && MiscUtils.KillersAliveCount == 1;
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
}
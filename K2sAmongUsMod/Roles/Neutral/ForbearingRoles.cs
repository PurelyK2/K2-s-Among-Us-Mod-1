using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using K2AmongUs.Options.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Roles.Crewmate;
using MiraAPI.Modifiers;
using K2AmongUs.Assets;

namespace K2AmongUs.Roles.Neutral;

public sealed class ForbearingRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    public int numMeetingsSkipped = -1;

    public string RoleName => "Forbearing";
    public string LocaleKey => "Forbearing";
    public DoomableType DoomHintType => DoomableType.Fearmonger;
    public string RoleDescription => "You are patient, but irritated...";
    public string RoleLongDescription => "Decrease Your Cooldowns Each Meeting That Is Skipped Or Tied.";
    public string GetAdvancedDescription() { return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType()); }

    public Color RoleColor => new Color32(217, 84, 77, byte.MaxValue);
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.SinisterIntro,
        Icon = K2RoleIcons.Forbearing,
        CanUseVent = OptionGroupSingleton<ForbearingOptions>.Instance.ForbearingCanVent
    };
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<SheriffRole>();
    public override void OnVotingComplete()
    {
        if (MeetingHud.Instance.exiledPlayer == null)
        {
            Player.RpcAddModifier<TownOfUs.Modifiers.Game.Assailant.AssassinModifier>();

            if(numMeetingsSkipped < 0)
            {
                numMeetingsSkipped++;
                MiraAPI.Utilities.Helpers.CreateAndShowNotification(
                    "There is no decision, a killer has awoken...",
                    RoleColor,
                    new Vector3(0f, 1f, -20f),
                    null,
                    TouRoleIcons.Jackal.LoadAsset()
                );
            }
            else if(OptionGroupSingleton<ForbearingOptions>.Instance.RestlessEveryMeeting)
            {
                numMeetingsSkipped++;

                MiraAPI.Utilities.Helpers.CreateAndShowNotification(
                    "The Killer Is Getting Tired Of Waiting...",
                    RoleColor,
                    new Vector3(0f, 1f, -20f),
                    null,
                    TouRoleIcons.Jackal.LoadAsset()
                );
            }
        }
    }

    /// <inheritdoc/>
    public bool WinConditionMet()
    {
        int playersAlive = MiraAPI.Utilities.Helpers.GetAlivePlayers().Count;
        return !Player.Data.IsDead && (MiscUtils.KillersAliveCount == 1 && playersAlive <= 2);
    }
    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet() && gameOverReason != GameOverReason.CrewmatesByTask && gameOverReason != GameOverReason.CrewmatesByVote;
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
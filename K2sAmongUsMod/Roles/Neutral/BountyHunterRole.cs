using AmongUs.GameOptions;
using HarmonyLib;
using K2AmongUs.Assets;
using K2AmongUs.Modifiers.Crewmate;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Events;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace K2AmongUs.Roles.Neutral;

public sealed class BountyHunterRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public string RoleName => "Bounty Hunter";
    public string LocaleKey => "Bounty Hunter";
    public DoomableType DoomHintType => DoomableType.Relentless;
    public string RoleDescription => "Chose Who Dies";
    public string RoleLongDescription => "Pick A Person To Be Targeted Next Round And Get The Evils To Kill Them";
    public string GetAdvancedDescription() { return "During The Meeting, Pick A Person To Be Targeted Next Round. If The Targeted Person Is Killed, Their Killer Gets A Predetermined \"Reward\".\n" + TownOfUs.Utilities.MiscUtils.AppendOptionsText(base.GetType()); }
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralEvil;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;

    public CustomRoleConfiguration Configuration => new(this)
    {
        IconTmp = TmpSpriteUtils.CreateSpriteAsset(K2RoleIcons.BountyHunter.LoadAsset(), "K2AmongUs.Roles.Neutral.BountyHunter", 1.45f),
        IntroSound = TouAudio.SinisterIntro,
        Icon = K2RoleIcons.BountyHunter,
    };

    public Color RoleColor => new Color32(10, 47, 14, byte.MaxValue);

    #region Meeting Stuff
    private MeetingMenu? meetingMenu;
    private NetworkedPlayerInfo? selectedPlr;
    public override void OnMeetingStart()
    {
        meetingMenu = new MeetingMenu(
            Player.Data.Role,
            Click,
            MeetingAbilityType.Toggle,
            K2Assets.BountyTarget,
            TouAssets.Guess,
            IsExempt,
            Color.white)
        {
            Position = new Vector3(-0.40f, 0f, -3f)
        };

        var meeting = MeetingHud.Instance;
        if (Player.AmOwner && meeting != null)
        {
            meetingMenu.GenButtons(meeting,
                Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());
            if (selectedPlr != null)
            {
                meetingMenu.Actives[selectedPlr.PlayerId] = true;
            }
        }
    }

    public void OnVotingComplete()
    {
        if (Player.AmOwner)
        {
            meetingMenu?.HideButtons();
        }

        AssignBountyTarget(selectedPlr);
    }

    public void Click(PlayerVoteArea voteArea, MeetingHud __)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.PlayerId);

        if (selectedPlr == player)
        {
            selectedPlr = null;
            meetingMenu?.Actives[voteArea.PlayerId] = false;
            return;
        }

        if (selectedPlr != null)
        {
            meetingMenu?.Actives[selectedPlr.PlayerId] = false;
            selectedPlr = null;
        }

        meetingMenu?.Actives[voteArea.PlayerId] = true;
        selectedPlr = player;
    }

    private bool IsExempt(PlayerVoteArea voteArea)
    {
        return false;
    }
    #endregion

    static void AssignBountyTarget(NetworkedPlayerInfo player)
    {
        PlayerControl? targetedPlayer = PlayerControl.AllPlayerControls.ToArray().First(p => p.Data.PlayerId == player?.PlayerId);

        if(targetedPlayer == null)
        {
            Error("No Bounty Target");
            return;
        }

        targetedPlayer.RpcAddModifier<BountyTargetModifier>();
    }
}

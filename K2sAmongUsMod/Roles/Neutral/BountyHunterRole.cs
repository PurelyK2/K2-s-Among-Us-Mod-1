/*
using AmongUs.GameOptions;
using HarmonyLib;
using K2AmongUs.Assets;
using K2AmongUs.Options.Roles.Neutral;
using K2sAmongUsMod.Modifiers.HiddenModifiers;
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
    private MeetingMenu? _meetingMenu;
    private NetworkedPlayerInfo? _selectedPlr;
    public override void OnRoleSet()
    {
        base.OnRoleSet();

        if (Player.AmOwner)
        {
            var classic = LegacyAssets.IsLegacy;
            _meetingMenu = new MeetingMenu(
                Player.Data.Role,
                Click,
                MeetingAbilityType.Toggle,
                classic ? LegacyAssets.ImitateSelectSprite : TouAssets.ImitateSelectSprite,
                classic ? LegacyAssets.ImitateDeselectSprite : TouAssets.ImitateDeselectSprite,
                IsExempt,
                Color.white)
            {
                Position = new Vector3(-0.40f, 0f, -3f)
            };
        }
    }
    public override void OnMeetingStart()
    {
        if (!Player.IsCrewmate())
        {
            Helpers.GetAlivePlayers().ForEach(delegate (PlayerControl player)
            {
                if(player.HasModifier<BountyTargetModifier>())
                {
                    player.RemoveModifier<BountyTargetModifier>();
                }
            });
            return;
        }

        var meeting = MeetingHud.Instance;
        if (Player.AmOwner && meeting != null)
        {
            _meetingMenu!.GenButtons(meeting,
                Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());
            if (_selectedPlr != null)
            {
                _meetingMenu!.Actives[_selectedPlr.PlayerId] = true;
            }
        }
    }

    public void OnVotingComplete()
    {
        if (Player.AmOwner)
        {
            _meetingMenu!.HideButtons();
        }
    }

    public void Click(PlayerVoteArea voteArea, MeetingHud __)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.PlayerId);

        if (_selectedPlr == player)
        {
            _selectedPlr = null;
            _meetingMenu!.Actives[voteArea.PlayerId] = false;
            return;
        }

        if (_selectedPlr != null)
        {
            _meetingMenu!.Actives[_selectedPlr.PlayerId] = false;
            _selectedPlr = null;
        }

        _meetingMenu!.Actives[voteArea.PlayerId] = true;
        _selectedPlr = player;
    }

    private bool IsExempt(PlayerVoteArea voteArea)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.PlayerId);
        if (Player.Data.IsDead || player == null || Player.PlayerId == voteArea.PlayerId || voteArea.AmDead)
        {
            return true;
        }
        return false;
    }
    #endregion
}
*/
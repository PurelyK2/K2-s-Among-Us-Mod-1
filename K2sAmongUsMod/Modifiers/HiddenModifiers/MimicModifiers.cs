/*
using AmongUs.GameOptions;
using HarmonyLib;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Assets;
using TownOfUs.Events;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modules;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace K2AmongUs.Modifiers.Neutral;

public sealed class MimicCacheModifier : BaseModifier, ICachedRole, IContinuesGame
{
    public bool CanDisplayForRole(RoleBehaviour role)
    {
        return !role.IsDead && role.Role != CachedRole.Role;
    }

    public bool ContinuesGame =>
        !Player.HasDied() && Player.IsCrewmate() && (MiscUtils.NKillersAliveCount > 0 || MiscUtils.ImpAliveCount > 0) &&
        MiscUtils.CrewKillersAliveCount == 0 && PlayerControl.AllPlayerControls.ToArray().Any(x =>
            x.Data.IsDead && x.GetRoleWhenAlive() is ITouCrewRole crewRole && crewRole.IsPowerCrew) &&
        Helpers.GetAlivePlayers().Count > 1;
    private MeetingMenu? _meetingMenu;
    private NetworkedPlayerInfo? _selectedPlr;
    public override string ModifierName => "Mimic";
    public string CachedRoleName => "Mimic";
    public override bool HideOnUi => false;
    public bool ShowCurrentRoleFirst => true;

    public bool Visible => Player.AmOwner || PlayerControl.LocalPlayer.HasDied() ||
                           FairyRole.FairySeesRoleVisibilityFlag(Player);

    public CacheRoleGuess GuessMode => (CacheRoleGuess)OptionGroupSingleton<MimicOptions>.Instance.MimicGuess;

    public RoleBehaviour CachedRole => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<MimicRole>());

    public override void OnActivate()
    {
        base.OnActivate();

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

    private bool IsExempt(PlayerVoteArea voteArea)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.PlayerId);
        if (Player.Data.IsDead || player == null || player.Object == null || voteArea.PlayerId == Player.PlayerId || player.Object.Data.Disconnected)
        {
            return true;
        }
        return false;
    }

    public override void OnMeetingStart()
    {
        if (Player.HasDied() && Player.AmOwner)
        {
            ModifierUtils.GetActiveModifiers<MimicedRevealedModifier>().Do(x => x.Player.RemoveModifier(x));
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

    public override void OnDeactivate()
    {
        _selectedPlr = null;

        if (Player.AmOwner && _meetingMenu != null)
        {
            _meetingMenu?.Dispose();
            _meetingMenu = null!;
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

    public void UpdateRole()
    {
        if (Player.HasDied())
        {
            return;
        }

        if (_selectedPlr == null || _selectedPlr.Disconnected || _selectedPlr.Object == null)
        {
            _selectedPlr = null;
            if (!Player || Player.IsRole<MimicRole>())
            {
                return;
            }

            Player.RpcChangeRole(RoleId.Get<MimicRole>(), false);
            return;
        }

        var roleWhenAlive = _selectedPlr.Object.GetRoleWhenAlive();

        if (roleWhenAlive is MimicRole || roleWhenAlive is SurvivorRole || roleWhenAlive.IsSimpleRole)
        {
            roleWhenAlive = RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<AurialRole>());
        }

        if (!_selectedPlr.Object.HasModifier<MimicedRevealedModifier>())
        {
            _selectedPlr.Object.AddModifier<MimicedRevealedModifier>(roleWhenAlive);
        }

        if (Player.Data.Role.GetType() != roleWhenAlive.GetType())
        {
            Player.RpcChangeRole((ushort)roleWhenAlive.Role, false);
        }
    }
}

public sealed class MimicedRevealedModifier : BaseRevealModifier
{
    private readonly RoleBehaviour _initialRole;

    public MimicedRevealedModifier(RoleBehaviour role)
    {
        _initialRole = role;
        ShownRole = role;
    }

    public override RoleBehaviour? ShownRole { get; set; }
    public override bool RevealRole { get; set; } = true;
    public override string ModifierName => "Role Revealed";
    public override void OnActivate()
    {
        base.OnActivate();
        var roleWhenAlive = Player.GetRoleWhenAlive();
        if (roleWhenAlive is ICrewVariant crewType)
        {
            roleWhenAlive = crewType.CrewVariant;
        }

        if (roleWhenAlive is ImitatorRole || roleWhenAlive is SurvivorRole || roleWhenAlive.IsSimpleRole)
        {
            roleWhenAlive = RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<ImitatorRole>());
        }
        SetNewInfo(true, null, null, roleWhenAlive);
    }

    public override void OnMeetingStart()
    {
        if (PlayerControl.LocalPlayer.HasDied())
        {
            Player.RemoveModifier(this);
            return;
        }
        var roleWhenAlive = Player.GetRoleWhenAlive();

        if (roleWhenAlive is MimicRole || roleWhenAlive is SurvivorRole || roleWhenAlive.IsSimpleRole)
        {
            roleWhenAlive = RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<MimicRole>());
        }
        SetNewInfo(true, null, null, roleWhenAlive);
        if (ShownRole == null)
        {
            return;
        }
        var meeting = MeetingHud.Instance;
        if (meeting == null)
        {
            return;
        }
        foreach (var voteArea in meeting.playerStates)
        {
            if (Player.PlayerId == voteArea.PlayerId)
            {
                Sprite? roleImg = null;

                if (ShownRole is ICustomRole customRole && customRole.Configuration.Icon != null)
                {
                    roleImg = customRole.Configuration.Icon.LoadAsset();
                }
                else if (ShownRole.RoleIconSolid != null)
                {
                    roleImg = ShownRole.RoleIconSolid;
                }

                if (roleImg != null)
                {
                    var newIcon = UnityEngine.Object.Instantiate(voteArea.PlayerIcon, voteArea.transform);
                    newIcon.gameObject.SetActive(true);
                    newIcon.transform.DestroyChildren();
                    var spriteRend = newIcon.GetComponent<SpriteRenderer>();
                    spriteRend.sprite = roleImg;
                    spriteRend.enabled = true;
                    newIcon.name = "RoleIcon";
                    newIcon.transform.localPosition = new Vector3(-1.25f, -0.15f, -3f);
                    newIcon.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                }

                break;
            }
        }
    }
}
*/
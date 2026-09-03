using K2AmongUs.Modifiers.Crewmate;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public sealed class MimicRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable, ICrewVariant
{
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Trickster;
    /// <inheritdoc/>
    public string RoleName => "Mimic";
    /// <inheritdoc/>
    public string RoleDescription => "Mimic Others Peoples's Abilities After Meetings To Win Alone";
    /// <inheritdoc/>
    public string RoleLongDescription => RoleDescription;
    /// <inheritdoc/>
    public Color RoleColor => Color.green;
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;

    /// <inheritdoc/>
    public RoleBehaviour CrewVariant => (RoleBehaviour)RoleId.Get<ImitatorRole>();

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.GlitchSound,
        Icon = TouAssets.TerminologySprite,
        TasksCountForProgress = false,
    };

    /// <inheritdoc/>
    public override void OnRoleSet()
    {
        if(Player.AmOwner && !Player.HasModifier<MimicRoleModifier>())
        {
            Player.AddModifier<MimicRoleModifier>();
        }
    }

    /// <inheritdoc/>
    public void OpenPickingUI()
    {
        CustomPlayerMenu playerMenu = CustomPlayerMenu.Create();
        playerMenu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material = PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        playerMenu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material = PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        playerMenu.Begin(delegate(PlayerControl plr)
        {
            return !plr.AmOwner;
        }, delegate (PlayerControl? plr)
        {
            playerMenu.ForceClose();
            if (plr != null)
            {
                MimicPlayer(plr);
            }
        });
        foreach (ShapeshifterPanel panel in playerMenu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            bool flag = panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName;
            if (flag)
            {
                panel.NameText.color = Color.white;
            }
        }
    }

    void MimicPlayer(PlayerControl player)
    {
        Player.RpcChangeRole(RoleId.Get(player.GetRoleWhenAlive().GetType()));

        if(!Player.HasModifier<MimicRoleModifier>())
            Player.AddModifier<MimicRoleModifier>();
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
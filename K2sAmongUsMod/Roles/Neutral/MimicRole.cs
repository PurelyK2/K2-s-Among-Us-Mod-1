using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using K2AmongUs.Modifiers.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TouExtensionExample;
using TouExtensionExample.Assets;
using TouExtensionExample.Buttons.Neutral;
using TownOfUs;
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
public sealed class MimicRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
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
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.GlitchSound,
        Icon = TouRoleIcons.Glitch
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
        }, delegate (PlayerControl plr)
        {
            playerMenu.ForceClose();
            if (plr != null)
            {
                Mimic(plr);
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

    void Mimic(PlayerControl player)
    {
        Player.RpcChangeRole(RoleId.Get(player.GetRoleWhenAlive().GetType()));

        if(!Player.HasModifier<MimicRoleModifier>())
            Player.AddModifier<MimicRoleModifier>();
    }
}
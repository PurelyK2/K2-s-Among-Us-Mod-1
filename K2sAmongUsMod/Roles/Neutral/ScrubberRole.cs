using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Roles.Neutral;
using MiraAPI.Modifiers;
using TownOfUs.Networking;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;

namespace K2AmongUs.Roles.Neutral;

/// <inheritdoc/>
public sealed class ScrubberRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    bool didWin = false;

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
        if(Player == null) return;

        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => !p.AmOwner).Select(p => p.GetModifiers<BaseModifier>()).Any(m => m.Any(m => !m.HideOnUi)))
            didWin = true;
    }

    public void OnRoundStart()
    {
        if(Player.AmOwner && didWin)
        {
            Player.RpcSpecialMurder(Player, true, true, true, false, false, false, false, false, "Cleansed");
            Player.RpcAddModifier<TownOfUs.Modifiers.BasicGhostModifier>();
        }
    }

    public bool GetDidWin()
    {
        return didWin;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return didWin;
    }

    [RegisterEvent(0)]
    public static void RoundStartHandler(RoundStartEvent @event)
    {
        if(@event.TriggeredByIntro) return;

        if(PlayerControl.LocalPlayer.Data.Role is ScrubberRole scrubber)
            scrubber.OnRoundStart();
    }
}
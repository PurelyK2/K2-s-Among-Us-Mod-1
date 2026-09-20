using Il2CppInterop.Runtime.Attributes;
using K2AmongUs.Assets;
using K2AmongUs.Modifiers.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers;
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
        IconTmp = TmpSpriteUtils.CreateSpriteAsset(K2RoleIcons.Scrubber.LoadAsset(), "K2AmongUs.Roles.Neutral.Scrubber", 1.45f),
        IntroSound = TouAudio.JanitorCleanSound,
        Icon = K2RoleIcons.Scrubber
    };
    public void OnRoundStart()
    {
        if (Player == null || Player.Data.IsDead || didWin || !Player.AmOwner) return;

        didWin = !ModifierUtils.GetPlayersWithModifier<BaseModifier>().Where(p => !p.Data.IsDead && p.Data.Role is not ScrubberRole).Any(p => p.GetModifiers<BaseModifier>().Any(m => !m.HideOnUi));

        if (didWin)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("The world has been cleansed of impurities, the only thing left to cleanse is yourself...", Color.white, new Vector3(0f, 1f, -20f), null, K2RoleIcons.Scrubber.LoadAsset());
        
            Player.RpcPlayerExile();
            Player.AddModifier<BasicGhostModifier>();
        }
    }
    public bool WinConditionMet()
    {
        return didWin;
    }

    public bool MetWinCon => didWin;

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



    [RegisterEvent(0)]
    public static void RoundStartHandler(RoundStartEvent @event)
    {
        if(@event.TriggeredByIntro) return;

        if(PlayerControl.LocalPlayer.Data.Role is ScrubberRole scrubber)
            scrubber.OnRoundStart();
    }

    [MethodRpc((uint) K2RpcCalls.ScrubModifiers)]
    public static void RpcScrubModifiers(PlayerControl scrubber, PlayerControl scrubbedPlayer)
    {
        foreach (BaseModifier modifier in scrubbedPlayer.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi))
        {
            scrubbedPlayer.RemoveModifier(modifier);
        }

        if (scrubbedPlayer.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your Modifiers Have Been Scrubbed", Color.white, new Vector3(0f, 1f, -20f), null, K2RoleIcons.Scrubber.LoadAsset());
        }
        if (scrubber.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Have Successfully Scrubbed " + scrubbedPlayer.Data.PlayerName + "'s Modifiers", Color.white, new Vector3(0f, 1f, -20f), null, K2RoleIcons.Scrubber.LoadAsset());
        }
    }
}
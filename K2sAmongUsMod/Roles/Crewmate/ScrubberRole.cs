using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Crewmate;

/// <inheritdoc/>
public sealed class ScrubberRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{

    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Protective;
    /// <inheritdoc/>
    public string LocaleKey => "Scrubber";
    /// <inheritdoc/>
    public string RoleName => "Scrubber";
    /// <inheritdoc/>
    public string RoleDescription => "Cleanse players of their modifiers.";
    /// <inheritdoc/>
    public string RoleLongDescription => RoleDescription + " (Can't Cleanse Jack-Of-All)";

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
    public Color RoleColor => new Color32(97, 147, 212, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.DenySound,
        Icon = TouRoleIcons.Amnesiac
    };

    /// <inheritdoc/>
    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
    }

    /// <inheritdoc/>
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
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
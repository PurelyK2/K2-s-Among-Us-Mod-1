using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
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
        IntroSound = TouAudio.JanitorCleanSound,
        Icon = TouRoleIcons.Infestor
    };
}
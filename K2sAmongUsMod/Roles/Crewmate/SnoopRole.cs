using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Roles.Crewmate;

/// <inheritdoc/>
public sealed class StealthyRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Trickster;
    /// <inheritdoc/>
    public string RoleName => "Snoop";
    /// <inheritdoc/>
    public string LocaleKey => RoleName;
    /// <inheritdoc/>
    public string RoleDescription => "Hide in plain sight and find the impostors";
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
				new("Sneak", "Hide in plain sight for an amount of time", TouRoleIcons.Chameleon),
            };
        }
    }

    /// <inheritdoc/>
    public Color RoleColor => new Color32(97, 147, 212, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.SwooperActivateSound,
        Icon = TouRoleIcons.Chameleon
    };
}
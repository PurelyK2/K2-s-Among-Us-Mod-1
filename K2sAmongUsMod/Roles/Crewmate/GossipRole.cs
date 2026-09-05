using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options.Roles.Crewmate;
using MiraAPI.GameOptions;

namespace K2AmongUs.Roles.Crewmate;

/// <inheritdoc/>
public sealed class GossipRole(IntPtr cppPtr) : CrewmateRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    static bool OnlyOneType = true;

    /// <inheritdoc/>
    public DoomableType DoomHintType => DoomableType.Insight;
    /// <inheritdoc/>
    public string LocaleKey => "Gossip";
    /// <inheritdoc/>
    public string RoleName => "Gossip";
    /// <inheritdoc/>
    public string RoleDescription => "Share some local lore!";
    /// <inheritdoc/>
    public string RoleLongDescription => "Overhear players, then gossip about them in the meeting.";

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
				new("Overhear", "Select A Player To Share Info About In The Meeting", TouModifierIcons.Crewpostor),
            };
        }
    }
    
    /// <inheritdoc/>
    public static void GenerateGossip(PlayerControl player, List<RoleBehaviour> randomRolesList)
    {
        string alertString = "Gossip Has Been Spread About " + player.Data.PlayerName + "! View Details In The Chat!";
        MiraAPI.Utilities.Helpers.CreateAndShowNotification(alertString, Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Crewpostor.LoadAsset());

        string gossipString = "";

        randomRolesList.Shuffle();

        foreach(string roleName in randomRolesList.Select(role => role.GetRoleName()))
        {
            gossipString += ", #" + roleName.Replace(" ", "-");
        }

        gossipString = gossipString.Substring(2);
        gossipString = player.Data.PlayerName + " is one of the following roles:\n" + gossipString;

        MiscUtils.AddFakeChat(player.Data, "Gossip:", gossipString, false, true);
    }
    
    /// <inheritdoc/>
    public Color RoleColor => new Color32(255, 237, 162, byte.MaxValue);
    /// <inheritdoc/>
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    /// <inheritdoc/>
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateInvestigative;

    /// <inheritdoc/>
    public CustomRoleConfiguration Configuration => new(this)
    {
        IntroSound = TouAudio.DetectiveIntroSound,
        Icon = TouModifierIcons.Colorblind
    };
}
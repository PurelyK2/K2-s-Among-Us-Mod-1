using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using K2AmongUs.Roles.Crewmate;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Crewmate;

/// <inheritdoc/>
public sealed class GossipOptions : AbstractOptionGroup<GossipRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Gossip";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Gossip Cooldown", 0f, 15f, 1f, MiraNumberSuffixes.Seconds)]
    public float GossipCooldown { get; set; } = 1f;
    
    /// <inheritdoc/>
    [ModdedNumberOption("Gossip Roles Count", 0f, 20f, 1f)]
    public float GossipRoles { get; set; } = 7f;

    /*
    // <inheritdoc/>
    [ModdedToggleOption("Gossip Shares Info")]
    public bool ShowGossip { get; set; } = true;
    */

    // <inheritdoc/>
    [ModdedNumberOption("Crew Role Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float CrewWeight { get; set; } = 30;
}
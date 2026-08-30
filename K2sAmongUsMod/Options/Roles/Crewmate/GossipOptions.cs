using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TouExtensionExample.Roles.Crewmate;
using TouExtensionExample.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace TouExtensionExample.Options.Roles.Crewmate;

/// <inheritdoc/>
public sealed class GossipOptions : AbstractOptionGroup<GossipRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Gossip";
    
    /// <inheritdoc/>
    [ModdedNumberOption("Gossip Cooldown", 0f, 60f, 5f, MiraNumberSuffixes.Seconds)]
    public float GossipCooldown { get; set; } = 15f;
    
    /// <inheritdoc/>
    [ModdedNumberOption("Gossip Roles Count", 0f, 20f, 1f)]
    public float GossipRoles { get; set; } = 7f;
}
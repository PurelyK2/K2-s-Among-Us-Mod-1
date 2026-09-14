using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using K2AmongUs.Roles.Crewmate;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Neutral;

public sealed class BountyHunterOptions : AbstractOptionGroup<BountyHunterRole>
{
    public override string GroupName => "Bounty Hunter Options";

    [ModdedNumberOption("Faction Modifier Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float randFactMod { get; set; } = 10f;
    [ModdedNumberOption("Universal Modifier Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float randUnivMod { get; set; } = 10f;
    /*
    [ModdedNumberOption("Lower Cooldowns Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float lowerCooldowns { get; set; } = 10f;
    */
    [ModdedNumberOption("Allow Venting Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float giveVentable { get; set; } = 10f;
    [ModdedNumberOption("Extra Vote Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float giveExtraVote { get; set; } = 10f;
    [ModdedNumberOption("Reveal Role Weight (CK Only)", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float revealCKRole { get; set; } = 10f;
    [ModdedNumberOption("Double Shot Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float giveDblShot { get; set; } = 10f;
    [ModdedNumberOption("Temporary Shield Weight", 0f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float shieldNextRound { get; set; } = 10f;
}

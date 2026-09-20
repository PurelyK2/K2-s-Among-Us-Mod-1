using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using K2AmongUs.Roles.Crewmate;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;

namespace K2AmongUs.Options.Roles.Impostor;

/// <inheritdoc/>
public sealed class DeceiverOptions : AbstractOptionGroup<DeceiverRole>
{
    public enum DeceiverRoleDisplayed
    {
        Investigator,
        RandomCrew
    }

    /// <inheritdoc/>
    public override string GroupName => "Deceiver Options";
    
    /// <inheritdoc/>
    [ModdedEnumOption("Deceiver Shows As", typeof(DeceiverRoleDisplayed), ["Investigator", "Random Crew"])]
    public DeceiverRoleDisplayed DeceiverDisplayedAs { get; set; } = DeceiverRoleDisplayed.Investigator;
}
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
public sealed class JackOfAllOptions : AbstractOptionGroup<JackOfAllRole>
{
    /// <inheritdoc/>
    public override string GroupName => "Jack-Of-All-Trades Options";

    /// <inheritdoc/>
    [ModdedNumberOption("Starting Modifiers", 0f, 10f, 1f)]
    public float NumModifiers { get; set; } = 5f;

    /// <inheritdoc/>
    [ModdedToggleOption("Can Get More Modifiers From Tasks")]
    public bool ModsFromTasks { get; set; } = true;

    /// <inheritdoc/>
    [ModdedNumberOption("Number of tasks per modifier", 1f, 5f, 1f)]
    public float TasksPerMod { get; set; } = 1f;
}
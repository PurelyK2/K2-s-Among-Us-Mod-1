using System;
using System.Runtime.CompilerServices;
using K2AmongUs.Modifiers.Game.Alliance;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modules.Localization;
using TownOfUs.Options.Modifiers;
using UnityEngine;

namespace K2AmongUs.Options.Modifiers.AllianceModifierOptions;

/// <inheritdoc/>
public sealed class RivalryOptions : AbstractOptionGroup<RivalryModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Rivalry Options";

    /// <inheritdoc/>
    [ModdedNumberOption("Rivalry Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float RivalsChance { get; set; } = 30f;

    /// <inheritdoc/>
    [ModdedNumberOption("Max Rivals Count", 2f, 15f, 1f)]
    public float RivalsCount { get; set; } = 2f;

    /// <inheritdoc/>
    [ModdedToggleOption("Disable Rivals Chat")]
    public bool RivalsChatOff { get; set; } = false;

    /// <inheritdoc/>
    [ModdedToggleOption("Rivals Know Each Other")]
    public bool RivalsKnowOthers { get; set; } = true;
}
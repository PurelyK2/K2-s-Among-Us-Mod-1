using K2AmongUs.Modifiers.Game.Universal;
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

namespace K2AmongUs.Options.Modifiers.UniversalModifierOptions;

    /// <inheritdoc/>
public sealed class HyperfocusOptions : AbstractOptionGroup<HyperfocusModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Hyperfocus Options";

    /// <inheritdoc/>
    [ModdedNumberOption("Hyperfocus Count", 0f, 5f, 1f)]
    public float HyperfocusCount { get; set; } = 1f;
    
    /// <inheritdoc/>
    [ModdedNumberOption("Hyperfocus Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float HyperfocusChance { get; set; } = 0f;
}
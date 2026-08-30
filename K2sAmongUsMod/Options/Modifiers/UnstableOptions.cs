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
public sealed class UnstableOptions : AbstractOptionGroup<UnstableModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Unstable Options";

    /// <inheritdoc/>
    [ModdedNumberOption("Unstable Count", 0f, 15f, 1f)]
    public float UnstableCount { get; set; } = 1f;

    /// <inheritdoc/>
    [ModdedNumberOption("Unstable Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float UnstableChance { get; set; } = 0f;

    /// <inheritdoc/>
    [ModdedNumberOption("Minimum TP Cooldown", 0f, 120f, 5f, MiraNumberSuffixes.Seconds)]
    public float UnstableMinCooldown { get; set; } = 30f;

    /// <inheritdoc/>
    [ModdedNumberOption("Maximum TP Cooldown", 0, 120f, 5f, MiraNumberSuffixes.Seconds)]
    public float UnstableMaxCooldown { get; set; } = 100f;
}
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
public sealed class BlindOptions : AbstractOptionGroup<BlindModifier>
{
    /// <inheritdoc/>
    public override string GroupName => "Blind Options";

    /// <inheritdoc/>
    [ModdedNumberOption("Blind Count", 0f, 5f, 1f)]
    public float BlindCount { get; set; } = 1f;
    
    /// <inheritdoc/>
    [ModdedNumberOption("Blind Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float BlindChance { get; set; } = 0f;

    /// <inheritdoc/>
    [ModdedNumberOption("Blind Amount", 5f, 100f, 5f, MiraNumberSuffixes.Percent)]
    public float BlindAmount { get; set; } = 30f;
}
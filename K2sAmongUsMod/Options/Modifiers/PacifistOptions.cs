using K2AmongUs.Modifiers.Game.Universal;
using System;
using System.Runtime.CompilerServices;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Modules.Localization;
using TownOfUs.Options.Modifiers;
using UnityEngine;
using K2AmongUs.Modifiers;

namespace K2AmongUs.Options.Modifiers.UniversalModifierOptions;

public sealed class PacifistOptions : AbstractOptionGroup<PacifistModifier>
{
    public override string GroupName => "Pacifist Options";

    [ModdedNumberOption("Pacifist Chance", 0f, 100f, 10f, MiraNumberSuffixes.Percent)]
    public float PacifistChance { get; set; } = 10f;
}
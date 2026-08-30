using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.ModifierDisplay;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.GameOver;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Roles.Other;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using TouExtensionExample;
using TownOfUs.Modifiers;

namespace K2AmongUs.Modifiers.Game.Universal;

/// <inheritdoc/>
public sealed class HyperfocusModifier : TouGameModifier, IWikiDiscoverable
{
    /// <inheritdoc/>
    public override string ModifierName => "Hyperfocus";
    /// <inheritdoc/>
    public override string LocaleKey => "Hyperfocus";
    /// <inheritdoc/>
    public override string IntroInfo => "You can see nothing but tasks in tasks";
    /// <inheritdoc/>
    public override string GetDescription()
    {
        return IntroInfo;
    }
    /// <inheritdoc/>
    public string GetAdvancedDescription()
    {
        return GetDescription() + MiscUtils.AppendOptionsText(base.GetType());
    }
    /// <inheritdoc/>
    public override ModifierFaction FactionType => ModifierFaction.Crewmate;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<HyperfocusOptions>.Instance.HyperfocusChance;
    }
    /// <inheritdoc/>
    public override float IntroSize => 3f;
    /// <inheritdoc/>
    public override bool HideOnUi => false;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> ModifierIcon => TouModifierIcons.Scout;
    /// <inheritdoc/>
    public override int GetAmountPerGame()
    {
        return CustomAmount;
    }
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<HyperfocusOptions>.Instance.HyperfocusCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<HyperfocusOptions>.Instance.HyperfocusChance;
}
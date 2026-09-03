using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;
using TownOfUs.Modifiers;
using K2AmongUs.Options.Modifiers.Game.Universal;

namespace K2AmongUs.Modifiers.Game.Universal;

/// <inheritdoc/>
public sealed class VentableModifier : TouGameModifier, IWikiDiscoverable
{
    /// <inheritdoc/>
    public override string ModifierName => "Ventable";
    /// <inheritdoc/>
    public override string LocaleKey => "Ventable";

    /// <inheritdoc/>
    public override bool HideFromGuessing => true;
    
    /// <inheritdoc/>
    public override string IntroInfo => "You can vent!";

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
    public override ModifierFaction FactionType => ModifierFaction.UniversalUtility;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<VentableOptions>.Instance.VentableChance;
    }
    /// <inheritdoc/>
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return !role.CanVent && base.IsModifierValidOn(role);
    }
    /// <inheritdoc/>
    public override bool? CanVent()
    {
        return true;
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
    public override int CustomAmount => (int)OptionGroupSingleton<VentableOptions>.Instance.VentableCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<VentableOptions>.Instance.VentableChance;
}
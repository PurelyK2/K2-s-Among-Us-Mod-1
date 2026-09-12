using K2AmongUs.Assets;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers.Game.Universal;

/// <inheritdoc/>
public sealed class BlindModifier : TouGameModifier, IWikiDiscoverable
{
    /// <inheritdoc/>
    public override string ModifierName => "Blind";
    /// <inheritdoc/>
    public override string LocaleKey => "Blind";
    /// <inheritdoc/>
    public override string IntroInfo => "Your vision is reduced by " + (int)OptionGroupSingleton<BlindOptions>.Instance.BlindAmount + "%";
    /// <inheritdoc/>
    public override bool HideFromGuessing => true;
    /// <inheritdoc/>
    public override string GetDescription()
    {
        return IntroInfo;
    }
    public override ModifierUiConfiguration Configuration
    {
        get
        {
            return new ModifierUiConfiguration(UnityEngine.Color.grey, TmpSpriteUtils.CreateSpriteAsset(K2ModifierIcons.Blind.LoadAsset(), "TouMira.Modifier.Game.Universal.Blind", 1.45f));
        }
    }
    /// <inheritdoc/>
    public string GetAdvancedDescription()
    {
        return GetDescription() + MiscUtils.AppendOptionsText(base.GetType());
    }
    /// <inheritdoc/>
    public override ModifierFaction FactionType => ModifierFaction.UniversalVisibility;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<BlindOptions>.Instance.BlindChance;
    }
    /// <inheritdoc/>
    public override float IntroSize => 3f;
    /// <inheritdoc/>
    public override bool HideOnUi => false;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> ModifierIcon => K2ModifierIcons.Blind;
    /// <inheritdoc/>
    public override int GetAmountPerGame()
    {
        return CustomAmount;
    }
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<BlindOptions>.Instance.BlindCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<BlindOptions>.Instance.BlindChance;
}
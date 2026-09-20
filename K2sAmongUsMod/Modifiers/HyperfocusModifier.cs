using K2AmongUs.Assets;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;

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
    public string GetAdvancedDescription() { return IntroInfo + MiscUtils.AppendOptionsText(base.GetType()); }

    /// <inheritdoc/>
    public override ModifierFaction FactionType => ModifierFaction.Crewmate;

    public override ModifierUiConfiguration Configuration
    {
        get
        {
            return new ModifierUiConfiguration(new UnityEngine.Color32(0, 60, 95, Byte.MaxValue), TmpSpriteUtils.CreateSpriteAsset(K2ModifierIcons.Blind.LoadAsset(), "TouMira.Modifier.Game.Universal.Blind", 1.45f));
        }
    }
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
    public override LoadableAsset<Sprite> ModifierIcon => TouAssets.TerminologySprite;
    /// <inheritdoc/>
    public override int GetAmountPerGame()
    {
        return CustomAmount;
    }
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<HyperfocusOptions>.Instance.HyperfocusCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<HyperfocusOptions>.Instance.HyperfocusChance;

    /// <inheritdoc/>
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return role.IsCrewmate();
    }
}
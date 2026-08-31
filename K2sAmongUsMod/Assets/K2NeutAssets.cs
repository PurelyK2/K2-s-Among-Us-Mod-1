using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace K2AmongUs.Assets;

/// <inheritdoc/>
public static class ExampleNeutAssets
{
    // THIS FILE SHOULD ONLY HOLD BUTTONS AND ROLE BANNERS, EVERYTHING ELSE BELONGS IN K2Assets.cs
    private const string ShortPath = "K2AmongUsMod.Resources.NeutButtons";
    /// <inheritdoc/>
    public static LoadableAsset<Sprite> SentinelVentSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelVentButton.png");
}
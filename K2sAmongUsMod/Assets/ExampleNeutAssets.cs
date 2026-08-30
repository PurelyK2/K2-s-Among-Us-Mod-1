using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TouExtensionExample.Assets;

public static class ExampleNeutAssets
{
    // THIS FILE SHOULD ONLY HOLD BUTTONS AND ROLE BANNERS, EVERYTHING ELSE BELONGS IN ExampleAssets.cs
    private const string ShortPath = "TouExtensionExample.Resources.NeutButtons";
    public static LoadableAsset<Sprite> SentinelVentSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelVentButton.png");
    public static LoadableAsset<Sprite> SentinelExplodeSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelExplodeButton.png");
    public static LoadableAsset<Sprite> SentinelKillSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelKillButton.png");
}
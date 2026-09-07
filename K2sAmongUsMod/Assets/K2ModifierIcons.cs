using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace K2AmongUs.Assets;

/// <inheritdoc/>
public static class K2ModifierIcons
{
    private const string ShortPath = "K2sAmongUsMod.Resources.ModifierIcons";

    public static LoadableAsset<Sprite> Blind { get; } = new LoadableResourceAsset($"{ShortPath}.Blind.png", 200);
    public static LoadableAsset<Sprite> Rivalry { get; } = new LoadableResourceAsset($"{ShortPath}.Rivalry.png", 200);
    public static LoadableAsset<Sprite> Unstable { get; } = new LoadableResourceAsset($"{ShortPath}.Unstable.png", 200);
    public static LoadableAsset<Sprite> Ventable { get; } = new LoadableResourceAsset($"{ShortPath}.Ventable.png", 200);
}
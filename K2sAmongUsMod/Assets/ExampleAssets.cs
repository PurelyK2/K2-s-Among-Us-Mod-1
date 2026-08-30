using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TouExtensionExample.Assets;

public static class ExampleAssets
{
    private const string ShortPath = "TouExtensionExample.Resources";
    public static LoadableAsset<Sprite> Banner { get; } = new LoadableResourceAsset($"{ShortPath}.ExampleBanner.png");
}

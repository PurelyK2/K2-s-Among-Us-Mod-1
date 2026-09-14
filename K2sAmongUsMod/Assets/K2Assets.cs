using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace K2AmongUs.Assets;

/// <inheritdoc/>
public static class K2Assets
{
    private const string ShortPath = "K2sAmongUsMod.Resources.Other";
    public static LoadableAsset<Sprite> BountyTarget { get; } = new LoadableResourceAsset($"{ShortPath}.Bounty Target.png", 200);
}

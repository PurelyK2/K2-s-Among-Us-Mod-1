using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace K2AmongUs.Assets;

/// <inheritdoc/>
public static class K2RoleIcons
{
    // THIS FILE SHOULD ONLY HOLD ROLE ICONS

    private const string ShortPath = "K2sAmongUsMod/Resources";

    // Neutrals
    /// <inheritdoc/>
    public static LoadableAsset<Sprite> Gossip { get; } = new LoadableResourceAsset($"{ShortPath}/RoleIcons/Gossip.png");
    /// <inheritdoc/>
    public static LoadableAsset<Sprite> JackOfAll { get; } = new LoadableResourceAsset($"{ShortPath}/RoleIcons/JOAR.png");
    /// <inheritdoc/>
    public static LoadableAsset<Sprite> Scrubber { get; } = new LoadableResourceAsset($"{ShortPath}/RoleIcons/Scrubber.png");
}
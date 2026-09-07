using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace K2AmongUs.Assets;

/// <inheritdoc/>
public static class K2RoleIcons
{
    // THIS FILE SHOULD ONLY HOLD ROLE ICONS

    private const string ShortPath = "K2sAmongUsMod.Resources.RoleIcons";

    public static LoadableAsset<Sprite> Mimic { get; } = new LoadableResourceAsset($"{ShortPath}.Mimic.png", 200);
    public static LoadableAsset<Sprite> Zombie { get; } = new LoadableResourceAsset($"{ShortPath}.Zombie.png", 200);
    public static LoadableAsset<Sprite> ZombieLeader { get; } = new LoadableResourceAsset($"{ShortPath}.ZombieLeader.png", 200);
}
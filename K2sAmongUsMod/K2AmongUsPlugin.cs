using System.Globalization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using K2AmongUs.Assets;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TownOfUs;
using UnityEngine;

namespace K2AmongUs;

/// <inheritdoc/>
[BepInAutoPlugin("com.K2sAmongUs.mod", "K2sAmongUsMod", "0.1.6")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[BepInDependency(TownOfUsPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class K2AmongUsPlugin : BasePlugin, IMiraPlugin
{
    /// <inheritdoc/>
    public static CultureInfo Culture => TownOfUs.TownOfUsPlugin.Culture;

    /// <inheritdoc/>
    public string OptionsTitleText => "K2's Mod";

    /// <inheritdoc/>
    public static bool IsDevBuild => false;

    /// <inheritdoc/>
    public ConfigFile GetConfigFile()
    {
        return Config;
    }

    /// <inheritdoc/>
    public Harmony Harmony { get; } = new(Id);

    /// <inheritdoc/>
    public override void Load()
    {
        ReactorCredits.Register("K2's Mod", Version, IsDevBuild, ReactorCredits.AlwaysShow);
        IL2CPPChainloader.Instance.Finished += Modules.ExtensionLocale.SearchInternalLocale;

        try
        {
            Harmony.PatchAll();
        }
        catch(System.Exception e)
        {
            _ = ConstantlyError(e.ToString());
        }
    }
    private static async Task ConstantlyError(string e)
    {
        while(true)
        {
            await Task.Delay(100);
            Fatal(e);
            
            if(Time.deltaTime > 1) break;
        }
    }
}

// Rival icons can't be seen in-game
// Rivals Chat Only For First Round?
// Everyone Sees Rivals Chats
// Adjust rivals desc
// End game if 1 of rivals is vig and the other can't kill and is crew

// ================ OTHER ================

// Add boo's bounty hunter idea

// Imp that can Hijack tasks?

// Sly but imp (random not-in-play crew for every )
// Neut outlier that needs to guess exactly 1 person's role

// =============== FIXES ===============
/*
Fixed Zombie Win Condition (THIS TIME FOR SURE!!!)
Addes Sprites to sprite folders
FINALLY added sprites for role/modifier icons (at least the ones that are done
Synced Rivals
Made Rivals Unguessable
Adjusted Snoop Options To Not Have Unused "Max Sneaks" Option (I will be implementing it, just not yet because there are more important things)
Snoop Should No Longer Be Affected By Mushroom Mixup While Snoopingss
*/
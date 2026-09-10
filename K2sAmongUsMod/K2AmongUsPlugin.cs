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
[BepInAutoPlugin("com.K2sAmongUs.mod", "K2sAmongUsMod", "0.1.8")]
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
// Rivals Aren't Synced
// Everyone Sees Rivals Chats
// End game if 1 of rivals is vig and the other can't kill and is crew

// FIX RIVALS!!!
// Finish Mimic
// Gossip Doesn't Share Info

// ================ OTHER ================

// Make Zombies Able To Talk As If Dead (Toggleable?)

// Make Outlines For Zombie Leader, Zombie, Gossip, And Forbearing Icons

// Add boo's bounty hunter idea
// Add Jay's Deceiver Idea

// Cleric can revive the zombie as survivor

// Make a "Scrubbed" Modifier So People Know Their Mods Were Scrubbed

// Add Extroverted And Introverted Cooldown Modifiers

// =============== FIXES ===============
/*
 * Added Percent Symbol To "Crew Role Weight" Gossip Option
 * Changed Gossip Cooldowns To Increment By 1 And Go Between 0 And 15
 * Made Zombies Spawn Normally
 * (Hopefully) Fixed Zombie Role Being Assigned Many Times Over
 * Zombie Leader Now Gets A Screen Flash When A Body Is Created
 * Gossip And Forbearing Icons Are Now Correctly Sized
 * Forbearing And Zombie Leader SHOULD No Longer Win With Crew... :sob:
 * Scrubber Should Now Win Correctly
 * Let Neutral Rivals Still Win With Their Win Condition
 * Made Zombie Role Visible To All
 * Made Zombies Revive Properly
 * Replaced Sly Modifier With Deceiver Impostor Role
*/
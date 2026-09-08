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
[BepInAutoPlugin("com.K2sAmongUs.mod", "K2sAmongUsMod", "0.1.7")]
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
// Adjust rivals desc
// End game if 1 of rivals is vig and the other can't kill and is crew
// Make Neutrals win by rival or win condition

// FIX RIVALS!!!
// Finish Mimic
// Gossip Doesn't Share Info
// Snoop Doesn't Have Max Sneaks

// Snoop has "can't use" icon for comms even though they can use in comms

// Scrubber Win Condition Is Bugged Still... :sob:


// ================ OTHER ================

// Add boo's bounty hunter idea
// Add Jay's Deceiver Idea

// Cleric can revive the zombie as survivor

// Make a "Scrubbed" Modifier So People Know Their Mods Were Scrubbed

// Add Extroverted And Introverted Cooldown Modifiers

// =============== FIXES ===============
/*
 * Fixed Typo For Forbearing
 * Jack Of All Role Can't Get Giant Or Mini From Tasks
 * HOPEFULLY Fixed Scrubber's Win Con For Real This Time!
 * Added "Crew Role Weight" To Gossip Options
 * Fixed Bugs With Unstable Modifier
 * Fixed Zombie Win Condition (Actually This Time)
 * Zombie Leader is now guessable by Vigilante
 * Fixed Forbearing Options Name
 * Zombies Die And Revive Properly
 * Zombies Can't Talk To Living
 * Mimic Removed From Roles List For Now (Until Finished)
 * Added Sprites For Forbearing, Gossip, And Jack Of All
 * Made It So Rivals Can't Guess Other Rivals (Unless They Don't Know Each Other)
*/
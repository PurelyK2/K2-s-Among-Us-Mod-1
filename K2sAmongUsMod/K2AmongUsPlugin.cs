using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using K2AmongUs.Assets;
using K2sAmongUsMod.CommsPatches;
using MiraAPI;
using MiraAPI.PluginLoading;
using PerfectComms.Api;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using System.Globalization;
using TownOfUs;
using UnityEngine;

namespace K2AmongUs;

/// <inheritdoc/>
[BepInAutoPlugin("com.K2sAmongUs.mod", "K2sAmongUsMod", "0.1.11")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[BepInDependency(TownOfUsPlugin.Id)]
[BepInDependency("com.edgetel.perfectcomms", BepInDependency.DependencyFlags.SoftDependency)]
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

        PerfectCommsSetup();
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

    void PerfectCommsSetup()
    {
        if (!IL2CPPChainloader.Instance.Plugins.ContainsKey(
                "com.edgetel.perfectcomms"))
            return;

        PerfectCommsVoiceIntegration.Register();
    }
}

// Buttons Can Be Clicked During Sabotage But Have The Blocking Symbol
// Deceiver Ends Game If They Aren't Host And Are The Only Killer

// ================ OTHER ================

// Finish Mimic

// Make Zombies Able To Talk As If Dead (Toggleable?)

// Add boo's bounty hunter idea
// Add Jay's Deceiver Idea

// Cleric can revive the zombie as survivor

// Make a "Scrubbed" Modifier So People Know Their Mods Were Scrubbed (Visible so long as they have no mods

// Add Extroverted And Introverted Cooldown Modifiers

// =============== FIXES ===============
/*
 * Zombie Leader Will No Longer Flash Everyone's Screen On Player Death
 * Zombies Die Properly At Meeting Start And Revive Later Than Before
 * Deceiver Shows On End Screen Properly
 * Updated README to include credit for Jay for the INCREDIBLE mod icon (I Haven't Found Where It Goes Yet. XD)!!!
 * Adjusted Deceiver Code To Correctly Show On Game End Screen
 * Adjusted Deceiver Code To Correctly Work With Assassin
 * Made Deceiver Not Deceive After Death
 * Made Deceiver Show The Right Number Of Impostors On Game Start
 * Fixed Bug Where Deceiver Broke Game Even When Off
 * Made Deceiver Show As Deceiver To Self
*/
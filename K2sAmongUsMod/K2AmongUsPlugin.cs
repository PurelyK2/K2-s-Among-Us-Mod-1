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
[BepInAutoPlugin("com.K2sAmongUs.mod", "K2sAmongUsMod", "1.0.2")]
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

public enum K2RpcCalls : uint
{
    ScrubModifiers = 0
}

// Add Lower Cooldown To Bounty Hunter Stuff

// Deceiver Shows As Winner For Deceived People

// Disable Person With Bounty From Calling Meeting

// ================ OTHER ================

// Make README pretty

// Make mimic more like neutral ambassador
// Add Light Blade's Semi-Transparent Modifier

// Add Extroverted And Introverted Cooldown Modifiers

// Buttons Can Be Clicked During Sabotage But Have The Blocking Symbol

// =============== FIXES ===============
/*
 * Scrubber Now Has A Delay On Scrub
 * Fixed Gossip Spelling Error
*/
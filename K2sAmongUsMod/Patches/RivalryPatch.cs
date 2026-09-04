using System.Runtime.CompilerServices;
using HarmonyLib;
using MiraAPI.Modifiers;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Utilities;
using K2AmongUs.Modifiers.Game.Alliance;
using Reactor.Utilities.Extensions;
using TownOfUs.Modifiers;
using MiraAPI.GameOptions;
using K2AmongUs.Options.Modifiers.AllianceModifierOptions;

namespace TouExtensionExample.Patches;

/// <inheritdoc/>
public static class RivalsChatPatch
{
    /// <inheritdoc/>
    [HarmonyPatch(typeof(ChatController), "SendChat")]
    [HarmonyPrefix]
    public static bool SendChatPatch(ChatController __instance)
    {
        if (MeetingHud.Instance || ExileController.Instance || PlayerControl.LocalPlayer.Data.IsDead)
        {
            return true;
        }
        string text = __instance.freeChatField.Text.WithoutRichText();
        if (text.Length < 1 || text.Length > 301)
        {
            return true;
        }
        if (PlayerControl.LocalPlayer.HasModifier<RivalryModifier>(null))
        {
            if (PlayerControl.LocalPlayer.HasModifier<ParasiteInfectedModifier>(null) || PlayerControl.LocalPlayer.HasModifier<PuppeteerControlModifier>(null))
            {
                NetworkedPlayerInfo data = PlayerControl.LocalPlayer.Data;
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
                defaultInterpolatedStringHandler.AppendLiteral("<color=#");
                defaultInterpolatedStringHandler.AppendFormatted(RivalryModifier.RivalsColor.ToHtmlStringRGBA());
                defaultInterpolatedStringHandler.AppendLiteral(">");
                defaultInterpolatedStringHandler.AppendFormatted("Rivals");
                defaultInterpolatedStringHandler.AppendLiteral("</color>");
                MiscUtils.AddTeamChat(data, defaultInterpolatedStringHandler.ToStringAndClear(), "You are under control! Your message cannot be sent.", false, false, false, BubbleType.Jailor);
            }
            else
            {
                RivalryModifier.RpcSendRivalsChat(PlayerControl.LocalPlayer, text);
            }
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    [HarmonyPatch(typeof(PlayerRoleTextExtensions), "UpdateTargetSymbols", [typeof(string), typeof(PlayerControl), typeof(bool)])]
    [HarmonyPostfix]
    public static void RivalsSymbolPatch(ref string __result, PlayerControl player, bool hidden = false)
    {
        RivalsKnownDisplay.TryAppendRivalsSymbol(ref __result, player);
    }
}

/// <inheritdoc/>
internal static class RivalsKnownDisplay
{
    internal static string RivalIcon
    {
        get
        {
            string result;

            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
            defaultInterpolatedStringHandler.AppendLiteral("<color=#");
            defaultInterpolatedStringHandler.AppendFormatted("FF0000");
            defaultInterpolatedStringHandler.AppendLiteral("> ");
            defaultInterpolatedStringHandler.AppendFormatted("R");
            defaultInterpolatedStringHandler.AppendLiteral("</color>");
            result = defaultInterpolatedStringHandler.ToStringAndClear();

            return result;
        }
    }

    internal static bool LocalShouldSeeRivals(PlayerControl row)
    {
        PlayerControl localPlayer = PlayerControl.LocalPlayer;
        
        if(localPlayer == null || row == null || localPlayer.Data == null)
            return false;

        if(!row.HasModifier<RivalryModifier>())
            return false;

        if(DeathHandlerModifier.IsFullyDead(localPlayer))
            return true;

        if(localPlayer.HasModifier<RivalryModifier>() && OptionGroupSingleton<RivalryOptions>.Instance.RivalsKnowOthers)
            return true;

        return false;
    }

    internal static void TryAppendRivalsSymbol(ref string result, PlayerControl row)
    {
        if(RivalsKnownDisplay.LocalShouldSeeRivals(row) && !result.Contains(RivalsKnownDisplay.RivalIcon))
        {
            result += RivalsKnownDisplay.RivalIcon;
        }
    }
}
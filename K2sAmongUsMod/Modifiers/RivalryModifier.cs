using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options.Modifiers.AllianceModifierOptions;
using TouExtensionExample;
using TownOfUs.Modifiers.Neutral;
using MiraAPI.GameEnd;
using TownOfUs.Modifiers.Game.Alliance;
using TownOfUs.Roles.Other;
using System.Runtime.CompilerServices;
using Reactor.Utilities.Extensions;

namespace K2AmongUs.Modifiers.Game.Alliance;

/// <inheritdoc/>
public sealed class RivalryModifier : AllianceGameModifier, IWikiDiscoverable, IAssignableTargets
{
    /// <inheritdoc/>
    public static Color RivalsColor { get; } = Color.green;

    /// <inheritdoc/>
    public override bool HideOnUi => false;

    /// <inheritdoc/>
    public override string Symbol => "R";

    /// <inheritdoc/>
    public override string ModifierName => "Rivalry";
    /// <inheritdoc/>
    public override string LocaleKey => "Rival";
    /// <inheritdoc/>
    public override string IntroInfo => RivalsString();
    /// <inheritdoc/>
    public override string GetDescription()
    {
        return RivalsString();
    }
    /// <inheritdoc/>
    public string GetAdvancedDescription()
    {
        return "You win if you survive for longer than your rival(s)";
    }
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> ModifierIcon => TouRoleIcons.Haunter;
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<RivalryOptions>.Instance.RivalsCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<RivalryOptions>.Instance.RivalsChance;

    /// <inheritdoc/>
    public bool ForceDisableTasks { get; private set; }


    /// <inheritdoc/>
    public static List<PlayerControl> GetAllRivals()
    {
        return PlayerControl.AllPlayerControls.ToArray().Where(x => x.HasModifier<RivalryModifier>()).ToList();
    }

    /// <inheritdoc/>
    public override void OnActivate()
    {
        if (!base.Player.AmOwner)
        {
            return;
        }
        DestroyableSingleton<HudManager>.Instance.Chat.gameObject.SetActive(true);
        Sprite[] buttonArray = new Sprite[]
        {
            TouChatAssets.NormalChatIdle.LoadAsset(),
            TouChatAssets.NormalChatHover.LoadAsset(),
            TouChatAssets.NormalChatOpen.LoadAsset()
        };
        Transform chatTransform = DestroyableSingleton<HudManager>.Instance.Chat.chatButton.transform;
        chatTransform.Find("Inactive").GetComponent<SpriteRenderer>().sprite = buttonArray[0];
        chatTransform.Find("Inactive").GetComponent<SpriteRenderer>().color = RivalsColor;
        chatTransform.Find("Active").GetComponent<SpriteRenderer>().sprite = buttonArray[1];
        chatTransform.Find("Active").GetComponent<SpriteRenderer>().color = RivalsColor;
        chatTransform.Find("Selected").GetComponent<SpriteRenderer>().sprite = buttonArray[2];
        chatTransform.Find("Selected").GetComponent<SpriteRenderer>().color = RivalsColor;
    }

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        base.OnMeetingStart();
        if (!Player.AmOwner)
        {
            return;
        }
        Sprite[] buttonArray = new Sprite[]
        {
            TouChatAssets.NormalChatIdle.LoadAsset(),
            TouChatAssets.NormalChatHover.LoadAsset(),
            TouChatAssets.NormalChatOpen.LoadAsset()
        };
        Transform chatTransform = DestroyableSingleton<HudManager>.Instance.Chat.chatButton.transform;
        chatTransform.Find("Inactive").GetComponent<SpriteRenderer>().sprite = buttonArray[0];
        chatTransform.Find("Active").GetComponent<SpriteRenderer>().sprite = buttonArray[1];
        chatTransform.Find("Selected").GetComponent<SpriteRenderer>().sprite = buttonArray[2];
    }

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return CustomChance;
    }

    /// <inheritdoc/>
    public override int GetAmountPerGame()
    {
        return CustomAmount;
    }


    /// <inheritdoc/>
    public static string RivalsString()
    {
        return "Outlast your " + (GetAllRivals().Count - 1) + " rival(s) to win";
    }

    /// <inheritdoc/>
    public static bool WinConditionMet()
    {
        if(Helpers.GetAlivePlayers().Count((PlayerControl x) => x.HasModifier<RivalryModifier>() && !x.HasDied()) > 1)
            return false;

        if(!Helpers.GetAlivePlayers().Any((PlayerControl x) => x.HasModifier<RivalryModifier>()))
            return false;

        return true;
    }

    /// <inheritdoc/>
    public override bool? DidWin(GameOverReason reason)
    {
        return WinConditionMet();
    }

    /// <inheritdoc/>
    public static void RpcSendRivalsChat(PlayerControl sender, string text)
    {
        bool flag = LobbyBehaviour.Instance;
        if (!flag)
        {
            NetworkedPlayerInfo networkedPlayerInfo = PlayerControl.LocalPlayer.Data;
            ChatController chat = DestroyableSingleton<HudManager>.Instance.Chat;
            AudioClip messageSound = chat.messageSound;
            NetworkedPlayerInfo basePlayer = networkedPlayerInfo;
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
            defaultInterpolatedStringHandler.AppendLiteral("<color=#");
            defaultInterpolatedStringHandler.AppendFormatted(RivalsColor.ToHtmlStringRGBA());
            defaultInterpolatedStringHandler.AppendLiteral(">Rival</color>");
            MiscUtils.AddTeamChat(basePlayer, defaultInterpolatedStringHandler.ToStringAndClear(), text, false, !sender.AmOwner, false, BubbleType.None);
            chat.messageSound = messageSound;
            bool flag9 = PlayerControl.LocalPlayer != sender;
            if (flag9)
            {
                SoundManager.Instance.PlaySound(TouAudio.DenySound.LoadAsset(), false, 1f, null);
            }
        }
    }

    int IAssignableTargets.Priority { get; set; } = 5;
    /// <inheritdoc/>
    public void AssignTargets()
    {
        foreach(PlayerControl rival in PlayerControl.AllPlayerControls.ToArray().Where(x => x.HasModifier<RivalryModifier>()))
        {
            rival.RpcRemoveModifier<RivalryModifier>();
        }

        int chance = UnityEngine.Random.Range(1, 102);
        if(chance <= (int)OptionGroupSingleton<RivalryOptions>.Instance.RivalsChance)
        {
            RivalryOptions rivalryOptions = OptionGroupSingleton<RivalryOptions>.Instance;

            List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.HasModifier<AllianceGameModifier>() && !x.HasModifier<ExecutionerTargetModifier>() && !SpectatorRole.TrackedSpectators.Contains(x.Data.PlayerName)).ToList();

            int rivalsCount = (int)rivalryOptions.RivalsCount;

            for(int i = 0; i < rivalsCount; i++)
            {
                if(players.Count == 0)
                {
                    Error("Not enough players to select more rivals");
                    break;
                }

                int randNum = UnityEngine.Random.Range(0, players.Count);
                PlayerControl thisPlayer = players[randNum];
                players.Remove(thisPlayer);

                thisPlayer.AddModifier<RivalryModifier>();
            }
        }
    }

}
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs.Assets;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using UnityEngine;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using TownOfUs.Modules;
using TownOfUs.Modifiers;
using TownOfUs.Roles.Crewmate;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Game.Universal;

namespace K2AmongUs.Modifiers.Game.Universal;

/// <inheritdoc/>
public sealed class UnstableModifier : TouGameModifier, IWikiDiscoverable
{
    /// <inheritdoc/>
    public bool isUnstable { get; set; }

    /// <inheritdoc/>
    public override string ModifierName => "Unstable";
    
    /// <inheritdoc/>
    public override string LocaleKey => "Unstable";

    /// <inheritdoc/>
    public override string IntroInfo => "You are unstable";

    /// <inheritdoc/>
    public override bool HideFromGuessing => true;


    /// <inheritdoc/>
    public override string GetDescription()
    {
        int minTPTime = (int)OptionGroupSingleton<UnstableOptions>.Instance.UnstableMinCooldown;
        int maxTPTime = Mathf.Max(minTPTime, (int)OptionGroupSingleton<UnstableOptions>.Instance.UnstableMaxCooldown);

        if(minTPTime == maxTPTime)
        {
            return "Randomly Teleport With Other Players Every " + minTPTime + " Seconds";
        }
        return "Randomly Teleport With Other Players Every " + minTPTime + " - " + maxTPTime + " Seconds";
    }
    /// <inheritdoc/>
    public string GetAdvancedDescription()
    {
        return "Randomly Teleport With Other Players Throughout The Round" + MiscUtils.AppendOptionsText(base.GetType());
    }
    /// <inheritdoc/>
    public override ModifierFaction FactionType => ModifierFaction.UniversalPassive;

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
    public override float IntroSize => 5f;
    /// <inheritdoc/>
    public override bool HideOnUi => false;
    /// <inheritdoc/>
    public override LoadableAsset<Sprite> ModifierIcon => TouNeutAssets.MimicSprite;
    /// <inheritdoc/>
    public override int CustomAmount => (int)OptionGroupSingleton<UnstableOptions>.Instance.UnstableCount;
    /// <inheritdoc/>
    public override int CustomChance => (int)OptionGroupSingleton<UnstableOptions>.Instance.UnstableChance;

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        if(base.Player.HasDied() && base.Player.AmOwner)
        {
            base.Player.RemoveModifier<UnstableModifier>();
            return;
        }
        isUnstable = false;
    }

    float tpTimer;

    /// <inheritdoc/>
    public override void Update()
    {
        base.Update();

        if(!Player.AmOwner) return;

        if(tpTimer <= 0)
        {
            RandomlyTeleport();
            tpTimer = ResetTPTimer();
        }
        else
        {
            tpTimer -= Time.deltaTime;
        }
    }


    /// <inheritdoc/>
    void RandomlyTeleport()
    {
        if(!(MeetingHud.Instance || ExileController.Instance))
        {
            System.Collections.Generic.List<PlayerControl> playersList = Helpers.GetAlivePlayers().Where(pl => pl != Player && !pl.HasModifier<ImmovableModifier>()).ToList();
            if(playersList.Count > 0)
            {
                playersList.Shuffle();
                
                PlayerControl randPlayer = playersList[UnityEngine.Random.Range(0, playersList.Count)];

                ushort roleId = RoleId.Get(Player.GetRoleWhenAlive().GetType());
                Player.RpcChangeRole(RoleId.Get<TransporterRole>(), false);
                TransporterRole.RpcTransport(Player, Player.PlayerId, randPlayer.PlayerId);
                Player.RpcChangeRole(roleId, false);
            
                Info("Transported In An Unstable Way");
            }
        }
    }
    static float ResetTPTimer()
    {
        float minTPTime = OptionGroupSingleton<UnstableOptions>.Instance.UnstableMinCooldown;
        float maxTPTime = OptionGroupSingleton<UnstableOptions>.Instance.UnstableMaxCooldown;

        return UnityEngine.Random.Range(minTPTime, maxTPTime);
    }
}
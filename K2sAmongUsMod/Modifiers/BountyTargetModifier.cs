using HarmonyLib;
using Il2CppSystem.Web.Util;
using K2AmongUs.Assets;
using K2AmongUs.Modifiers;
using K2AmongUs.Modifiers.Crewmate;
using K2AmongUs.Modifiers.Game.Universal;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Assets;
using TownOfUs.Events;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modifiers.Game.Assailant;
using TownOfUs.Modifiers.Game.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modules.RainbowMod;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers;

public sealed class BountyTargetModifier : AllianceGameModifier
{
    public override string ModifierName => "Bounty Target";
    public override string GetDescription()
    {
        return "You Are The Bounty Hunter Target...\nGood Luck!";
    }

    public override void OnActivate()
    {
        base.OnActivate();

        if (Player.Data.IsDead || !MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.Data.Role is BountyHunterRole))
        {
            ModifierComponent.RemoveModifier(this);
            return;
        }

        if (Player.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("A Bounty Has Been Placed On You...", UnityEngine.Color.gray, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
        }
        else if (ShouldGetBountyNotif(PlayerControl.LocalPlayer))
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("A Bounty Has Been Placed On " + Player.Data.PlayerName + "'s Head.\nKill Them To Get A Reward!", UnityEngine.Color.red, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
            Player.AddModifier<BountyArrowModifier>(PlayerControl.LocalPlayer, Player.Data.Color, 0f);
        }
    }

    public void Update()
    {
        if (Player.Data.IsDead || !MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.Data.Role is BountyHunterRole))
        {
            if (ShouldGetBountyNotif(PlayerControl.LocalPlayer))
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("The Bounty Hunter Has Died, They Can No Longer Give A Reward...", UnityEngine.Color.red, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
        
            ModifierComponent.RemoveModifier(this);
        }
    }

    public override void OnMeetingStart()
    {
        base.OnMeetingStart();
        ModifierComponent.RemoveModifier(this);
    }

    static bool ShouldGetBountyNotif(PlayerControl player)
    {
        return player.Data.Role.GetRoleAlignment() == TownOfUs.Roles.RoleAlignment.CrewmateKilling || !player.Data.Role.IsCrewmate() || player.Data.IsDead;
    }
    static void GivePlayerBonus(PlayerControl player, PlayerControl target)
    {
        if (ShouldGetBountyNotif(PlayerControl.LocalPlayer) && !player.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("The Bounty Has Been Claimed...", UnityEngine.Color.red, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
        }
        
        if (player.AmOwner)
            player.AddModifier<BountyRewardModifier>();
    }

    [RegisterEvent(0)]
    public static void PlayerDeathEventHandler(BeforeMurderEvent @event)
    {
        Info("Noticed Player Death");
        if (@event.Target.HasModifier<BountyTargetModifier>())
            GivePlayerBonus(@event.Source, @event.Target);
    }

    public override int GetAssignmentChance()
    {
        return 0;
    }
}
public sealed class BountyArrowModifier(PlayerControl owner, Color color, float update) : ArrowTargetModifier(owner, color, update)
{
    public override string ModifierName => "Bounty Arrow";

    public override void OnActivate()
    {
        base.OnActivate();

        if (Arrow == null)
        {
            return;
        }

        var spr = Arrow.gameObject.GetComponent<SpriteRenderer>();
        var r = Arrow.gameObject.AddComponent<BasicRainbowBehaviour>();

        r.AddRend(spr, Player.cosmetics.ColorId);
    }

    public override void OnMeetingStart()
    {
        base.OnMeetingStart();

        ModifierComponent.RemoveModifier(this);
    }

    public override void OnDeath(DeathReason reason)
    {
        TouAudio.PlaySound(TouAudio.TrackerDeactivateSound);

        base.OnDeath(reason);
    }
}

public sealed class BountyRewardModifier : TouGameModifier
{
    public override string ModifierName => "Bounty Reward Modifier";

    public override int GetAssignmentChance()
    {
        return 0;
    }

    public override void OnActivate()
    {
        base.OnActivate();

        DeceiverRole.confuseRole = false;
        GiveRandomBounty();
        DeceiverRole.confuseRole = true;
    }

    void GiveRandomBounty()
    {
        if (!Player.AmOwner)
        {
            ModifierComponent.RemoveModifier(this);
            return;
        }

        MiraAPI.Utilities.Helpers.CreateAndShowNotification("You killed the Bounty Hunter Target", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
        BountyHunterOptions opts = OptionGroupSingleton<BountyHunterOptions>.Instance;

        RoleAlignment thisFaction = Player.Data.Role.GetRoleAlignment();
        bool canVent = Player.HasModifier<VentableModifier>() || Player.Data.Role.CanVent;
        bool canGetDouble = !Player.HasModifier<DoubleShotModifier>() && Player.HasModifier<AssassinModifier>();

        List<RewardType> rewards = new List<RewardType>();

        // Random Good Modifier From Your Faction
        if(Player.Data.Role.IsCrewmate())
            for (int i = 0; i < (int)opts.randFactMod; i++)
            {
                rewards.Add(RewardType.GoodFactMod);
            }

        // Give Random Good Universal Modifier
        for (int i = 0; i < (int)opts.randUnivMod; i++)
        {
            rewards.Add(RewardType.GoodUnivMod);
        }

        // Universal Decreased Cooldowns (Like Mage Energize?)
        /*
        for (int i = 0; i < (int)opts.lowerCooldowns; i++)
        {
            rewards.Add(RewardType.LowerCooldown);
        }
        */

        // Give Ventable (if can't vent)
        if(!canVent)
            for (int i = 0; i < (int)opts.giveVentable; i++)
            {
                rewards.Add(RewardType.GiveVentable);
            }

        // Extra Vote (JOA Extra Vote Modifier?)
        for (int i = 0; i < (int)opts.giveExtraVote; i++)
        {
            rewards.Add(RewardType.ExtraVote);
        }

        // Reveal Role (If Crew Killing)
        if(thisFaction == RoleAlignment.CrewmateKilling)
            for (int i = 0; i < (int)opts.revealCKRole; i++)
            {
                rewards.Add(RewardType.RevealRole);
            }

        // Double Shot (If you have assassin and no double shot)
        if(canGetDouble)
            for (int i = 0; i < (int)opts.giveDblShot; i++)
            {
                rewards.Add(RewardType.DoubleShot);
            }

        // Shield until end of next round
        for (int i = 0; i < (int)opts.shieldNextRound; i++)
        {
            rewards.Add(RewardType.GiveShield);
        }

        if(rewards.Count == 0)
        {
            Error("No Rewards Available");
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("The Bounty Hunter Is Too Poor To Give Handouts", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
            return;
        }


        rewards.Shuffle();
        RewardType thisReward = rewards[0];

        List<BaseModifier> modifiers = MiscUtils.AllModifiers.ToList();
        List<Type> possibleModifiers = new List<Type>();

        switch (thisReward)
        {
            case RewardType.GoodFactMod:
                possibleModifiers.Add(typeof(BaitModifier));
                possibleModifiers.Add(typeof(CelebrityModifier));
                possibleModifiers.Add(typeof(FrostyModifier));
                possibleModifiers.Add(typeof(InvestigatorModifier));
                possibleModifiers.Add(typeof(MultitaskerModifier));
                possibleModifiers.Add(typeof(NoisemakerModifier));
                possibleModifiers.Add(typeof(OperativeModifier));
                possibleModifiers.Add(typeof(ScientistModifier));
                possibleModifiers.Add(typeof(ScoutModifier));
                possibleModifiers.Add(typeof(SpyModifier));
                possibleModifiers.Add(typeof(TorchModifier));

                possibleModifiers.RemoveAll(m => Player.HasModifier(m));

                if(possibleModifiers.Count == 0)
                {
                    Error("No Possible Good Crewmate Modifiers To Give");
                    return;
                }

                possibleModifiers.Shuffle();
                Player.RpcAddModifier(possibleModifiers[0]);
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Got A Random Crewmate Modifier", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.GoodUnivMod:
                possibleModifiers.Add(typeof(ButtonBarryModifier));
                possibleModifiers.Add(typeof(TiebreakerModifier));
                possibleModifiers.Add(typeof(ImmovableModifier));
                possibleModifiers.Add(typeof(RadarModifier));
                possibleModifiers.Add(typeof(ShyModifier));
                possibleModifiers.Add(typeof(SixthSenseModifier));
                possibleModifiers.Add(typeof(SleuthModifier));

                possibleModifiers.RemoveAll(m => Player.HasModifier(m));

                if (possibleModifiers.Count == 0)
                {
                    Error("No Possible Good Universal Modifiers To Give");
                    return;
                }

                possibleModifiers.Shuffle();
                Player.RpcAddModifier(possibleModifiers[0]);
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Got A Random Universal Modifier", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.LowerCooldown:
                //Not Accessable Atm because not implemented
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your Cooldowns Are Decreased", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.GiveVentable:
                Player.RpcAddModifier<VentableModifier>();
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Can Now Vent", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.ExtraVote:
                if(!Player.HasModifier<JackOfAllVotes>())
                {
                    Player.RpcAddModifier<JackOfAllVotes>();
                }
                else
                {
                    JackOfAllVotes votes = Player.GetModifier<JackOfAllVotes>();
                    votes.NumVotes++;
                }
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Have An Extra Vote", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.RevealRole:
                Player.RpcAddModifier<BountyRevealModifier>();
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your Role Has Been Revealed To Everyone", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.DoubleShot:
                Player.RpcAddModifier<DoubleShotModifier>();
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Know Have Double Shot", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
            case RewardType.GiveShield:
                Player.RpcAddModifier<BountyShieldModifier>();
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Have A Temporary Shield", UnityEngine.Color.white, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
                break;
        }

        try
        {
            foreach (BountyHunterRole bountyHunter in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => p.Data.Role is BountyHunterRole).Select(p => p.Data.Role as BountyHunterRole))
            {
                if (bountyHunter != null)
                {
                    bountyHunter.NumBountiesCollected++;
                }
            }
        }
        catch (Exception e)
        {
            Error(e);
        }
    }

    enum RewardType
    {
        GoodFactMod,
        GoodUnivMod,
        LowerCooldown,
        GiveVentable,
        ExtraVote,
        RevealRole,
        DoubleShot,
        GiveShield
    }

    [HarmonyPatch(typeof(Vent), "Use")]
    public static class NoBountyHidingInVentsPatch
    {
        public static bool Prefix()
        {
            if(PlayerControl.LocalPlayer.HasModifier<BountyTargetModifier>())
                MiraAPI.Utilities.Helpers.CreateAndShowNotification("Don't Even Think About it...", UnityEngine.Color.red, new UnityEngine.Vector3(0f, 1f, -20f), null, K2RoleIcons.BountyHunter.LoadAsset());
            return !PlayerControl.LocalPlayer.HasModifier<BountyTargetModifier>();
        }
    }

    [RegisterEvent(0)]
    public static void OnEndGameEvent(GameEndEvent __)
    {
        foreach(PlayerControl player in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => p.HasModifier<BountyTargetModifier>()))
        {
            player.RpcRemoveModifier<BountyTargetModifier>();
        }
    }
}
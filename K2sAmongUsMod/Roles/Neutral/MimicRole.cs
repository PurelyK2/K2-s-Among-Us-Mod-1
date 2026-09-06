using K2AmongUs.Modifiers.Neutral;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using System.Text;
using TMPro;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Wiki;
using UnityEngine;
using TownOfUs.Roles.Neutral;
using TownOfUs.Roles;

namespace K2AmongUs.Roles.Neutral;

public sealed class MimicRole(IntPtr cppPtr) : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    public DoomableType DoomHintType => DoomableType.Perception;
    public string RoleName => "Mimic";

    public string RoleDescription => "Mimic Others To Win.";
    public string RoleLongDescription => "Mimic Another Player's Role In Meetings To Hide In Plain Sight.";

    public string GetAdvancedDescription()
    {
        return RoleLongDescription + TownOfUs.Utilities.MiscUtils.AppendOptionsText(GetType());
    }
    public bool CanShowSecondTab => true;

    public Color RoleColor => K2AmongUsColors.Mimic;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public RoleAlignment RoleAlignment => RoleAlignment.CrewmateSupport;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = TouNeutAssets.HackSprite,
        OptionsScreenshot = TouBanners.CrewmateRoleBanner,
        IntroSound = TouAudio.SpyIntroSound
    };

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        if (!player.HasModifier<MimicCacheModifier>())
        {
            player.AddModifier<MimicCacheModifier>();
        }
    }

    /// <inheritdoc/>
    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>()!;
        return console == null || console.AllowImpostor;
    }
}
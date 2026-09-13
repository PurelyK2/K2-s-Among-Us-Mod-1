using MiraAPI.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Assets;
using TownOfUs.Modifiers;

namespace K2sAmongUsMod.Modifiers.HiddenModifiers;

public sealed class BountyTargetModifier(PlayerControl owner) : ArrowTargetModifier(owner, owner.Data.Color, 0f)
{
    public override string ModifierName => "Bounty Target";
    public override string GetDescription()
    {
        return "You Are The Bounty Hunter Target...\nGood Luck!";
    }

    public override bool AutoStart => true;

    public override void OnMeetingStart()
    {
        base.OnMeetingStart();
        ModifierComponent.RemoveModifier(this);
    }

    public override void OnDeath(DeathReason reason)
    {
        base.OnDeath(reason);

        TouAudio.PlaySound(TouAudio.TrackerDeactivateSound, 1f);
        base.OnDeath(reason);
    }
}

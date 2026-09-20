using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Modifiers;

namespace K2AmongUs.Modifiers;

public sealed class BountyRevealModifier : BaseRevealModifier
{
    public override string ModifierName => "Bounty Reveal Modifier";

    public override string GetDescription()
    {
        return "Your Role Is Revealed To Everyone";
    }
    public override bool HideOnUi => false;
    public override bool AutoStart => true;
    public override ChangeRoleResult ChangeRoleResult => ChangeRoleResult.Nothing;
    public override RoleBehaviour? ShownRole => Player.Data.Role;
    public override bool RevealRole => true;
    public override bool Visible => true;
}

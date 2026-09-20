using K2AmongUs.Roles.Neutral;
using PerfectComms.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2sAmongUsMod.CommsPatches;

public class ZombieComms
{
    public static VoiceRuleResult ZombieVoiceRule(VoiceRuleContext ctx)
    {
        if (PlayerControl.LocalPlayer != null && ctx.Player != null && ctx.Player.Data.Role is ZombieRole)
        {
            if (PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role is ZombieRole)
            {
                return VoiceRuleResult.Pass;
            }
            else
            {
                return VoiceRuleResult.Mute("Not Sentient Enough");
            }
        }

        return VoiceRuleResult.Pass;
    }
}

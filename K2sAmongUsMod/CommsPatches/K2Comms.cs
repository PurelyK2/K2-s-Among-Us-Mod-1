using K2AmongUs.Roles.Neutral;
using PerfectComms.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace K2sAmongUsMod.CommsPatches;
internal static class PerfectCommsVoiceIntegration
{
    private const string Mod = "com.K2sAmongUs.mod";

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Register()
    {
        //PerfectCommsApi.RegisterVoiceRule(Mod, ctx => ZombieComms.ZombieVoiceRule(ctx));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Unregister()
        => PerfectCommsApi.Unregister(Mod);
}

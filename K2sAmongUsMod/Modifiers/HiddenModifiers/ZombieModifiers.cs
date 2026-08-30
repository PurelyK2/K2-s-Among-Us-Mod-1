using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Rewired;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers.Neutral;

/// <inheritdoc/>
public sealed class ZombieRevealedModifier : BaseRevealModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Zombie Revealed";
    /// <inheritdoc/>
    public override ChangeRoleResult ChangeRoleResult { get; set; } = ChangeRoleResult.Nothing;
    /// <inheritdoc/>
    public override RoleBehaviour ShownRole => Player.GetRoleWhenAlive();
    /// <inheritdoc/>
    public override bool RevealRole => OptionGroupSingleton<ZombieOptions>.Instance.ZombieShowsRole;
    /// <inheritdoc/>
    public override bool Visible => true;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}
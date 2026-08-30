
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Roles;
using TouExtensionExample.Roles.Crewmate;
using TownOfUs;

namespace K2AmongUs.Events.Crewmate
{
	public static class JackOfAllEvents
	{
		[RegisterEvent(0)]
		public static void CompleteTaskEvent(CompleteTaskEvent @event)
		{
			Info("Completed Task");
			JackOfAllRole.CheckAddModifier(@event.Player);
		}
	}
}
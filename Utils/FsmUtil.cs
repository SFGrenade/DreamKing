﻿using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;

namespace DreamKing.Utils
{
	public static class FsmUtil
	{
		public static void AddTransition(this PlayMakerFSM fsm, string stateName, string @event, string toState, bool newEvent = false)
		{
			FsmState state = fsm.Fsm.GetState(stateName);
			List<FsmTransition> list = state.Transitions.ToList<FsmTransition>();
			list.Add(new FsmTransition
			{
				ToState = toState,
				FsmEvent = (newEvent ? new FsmEvent(@event) : fsm.FsmEvents.FirstOrDefault((FsmEvent x) => x.Name == @event))
			});
			state.Transitions = list.ToArray();
		}
	}
}

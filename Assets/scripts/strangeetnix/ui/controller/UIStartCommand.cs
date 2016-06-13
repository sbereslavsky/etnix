//While this Context doesn't actually need a startup Command
//I almost always leave one here...just in case.
//In any sufficiently complex project, there's pretty much
//always **something** that needs to happen on startup.

using System;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace strangeetnix.ui
{
	public class UIStartCommand : Command
	{
		[Inject (ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		[Inject]
		public SwitchCanvasSignal switchCanvasSignal { get; set; }

		public override void Execute ()
		{
			Debug.Log ("UIStartCommand execute!");

			switchCanvasSignal.Dispatch (UIStates.MAIN);
		}
	}
}


﻿//When we start the module, take care of any startup requriements.

//This version is called only when GameContext runs in standalone mode.
//It instantiates the IME and puts a debugview on-screen

using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class GameIndependentStartCommand : Command
	{
		[Inject (ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		[Inject]	//This injection simply instantiates the game input
		public IInput input{ get; set; }


		public override void Execute ()
		{
			GameObject go = new GameObject ("debug_view");
			go.AddComponent<GameDebugView> ();
			go.transform.parent = contextView.transform;
		}
	}
}


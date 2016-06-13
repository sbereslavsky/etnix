//For the moment, user input comes via KeyboardInput or the OnscreenControlsView.
//OnscreenControlsView is a View, rather than an injectable, instantiable
//Class, so when we use it, IInput gets mapped to this class to fulfill the injection
//requirement.

//A different solution might be to work up some logic in your Context that avoids the
//IInput injection. I prefer this route, as it guarantees all my dependencies will
//be satisfied.

using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.impl;
using System.Collections;
using UnityEngine;

namespace strangeetnix.game
{
	public class NullInput : IInput
	{
		[Inject(ContextKeys.CONTEXT_DISPATCHER)]
		public IEventDispatcher dispatcher { get; set; }

		[Inject]
		public IRoutineRunner routinerunner { get; set; }

		[Inject]
		public GameInputSignal gameInputSignal{ get; set; } 

		[PostConstruct]
		public void PostConstruct()
		{
			routinerunner.StartCoroutine (update());
		}

		protected IEnumerator update()
		{
			while (true)
			{
				int input = GameInputEvent.NONE;
				if (Input.GetKeyDown (KeyCode.Escape))
				{
					input |= GameInputEvent.QUIT;
				}
				gameInputSignal.Dispatch (input);
				yield return null;
			}
		}
	}
}


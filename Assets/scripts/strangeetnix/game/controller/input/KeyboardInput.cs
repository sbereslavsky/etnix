//Input from the Keyboard using arrow keys and spacebar

//Note, BTW, that I'm using bitwise evaluation to handle the key input. If you're not familiar with
//bitwise operations, I suggest you look it up. It's very useful for just this sort of scenario.
//http://en.wikipedia.org/wiki/Bitwise_operation

//In this case, I specifically use the following:
// |= Add the value to the result
// ^= Remove the value from the result
// &  Test if the value appears in the result

using System;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.impl;
using System.Collections;
using UnityEngine;

namespace strangeetnix.game
{
	public class KeyboardInput : IInput
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
				if (Input.GetKeyDown (KeyCode.LeftControl))
				{
					input |= GameInputEvent.HIT;
				}

				if (Input.GetKey (KeyCode.LeftArrow))
				{
					input |= GameInputEvent.LEFT;
				} 
				if (Input.GetKey (KeyCode.RightArrow))
				{
					input |= GameInputEvent.RIGHT;
				}
				gameInputSignal.Dispatch (input);
				yield return null;
			}
		}
	}
}


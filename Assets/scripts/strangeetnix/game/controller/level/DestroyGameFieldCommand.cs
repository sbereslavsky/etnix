using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class DestroyGameFieldCommand : Command
	{
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		public override void Execute ()
		{
			//setup the game field
			if (injectionBinder.GetBinding<GameObject> (GameElement.GAME_FIELD) != null)
			{
				injectionBinder.Unbind<GameObject> (GameElement.GAME_FIELD);
				GameObject gameField = GameObject.Find (GameElement.GAME_FIELD.ToString());
				GameObject.Destroy (gameField);
			}
		}
	}
}


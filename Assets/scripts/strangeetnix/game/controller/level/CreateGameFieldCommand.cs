﻿//Instantiates and binds a special GAME_FIELD GameObject to parent the rest of the game elements.

using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class CreateGameFieldCommand : Command
	{
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{ get; set; }

		public override void Execute ()
		{
			Vector3 center = Vector3.zero;

			//setup the game field
			if (injectionBinder.GetBinding<GameObject> (GameElement.GAME_FIELD) == null)
			{
				GameObject gameField = new GameObject (GameElement.GAME_FIELD.ToString());
				gameField.AddComponent<GameFieldView> ();
				gameField.transform.localPosition = center;
				gameField.transform.SetParent(contextView.transform, false);

				//Bind it so we can use it elsewhere
				injectionBinder.Bind<GameObject> ().ToValue (gameField).ToName (GameElement.GAME_FIELD);
			}
		}
	}
}


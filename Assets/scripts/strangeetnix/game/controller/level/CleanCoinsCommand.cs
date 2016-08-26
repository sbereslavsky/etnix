using System;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace strangeetnix.game
{
	public class CleanCoinsCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		public override void Execute()
		{
			//Clean up coins
			DropCoinView[] coins = gameField.GetComponentsInChildren<DropCoinView> ();
			foreach (DropCoinView coinView in coins)
			{
				coinView.forceDestroySignal.Dispatch ();
			}
		}
	}
}


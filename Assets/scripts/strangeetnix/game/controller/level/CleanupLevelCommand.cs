//At the end of a level (and once at the start of the game), make sure we put all our toys away

using System;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using UnityEngine;

namespace strangeetnix.game
{
	public class CleanupLevelCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public IEnemyPoolManager enemyPoolManager{ get; set; }

		[Inject]
		public DestroyPlayerSignal destroyPlayerSignal{ get; set; }

		[Inject]
		public DestroyEnemySignal destroyEnemySignal{ get; set; }

		[Inject]
		public CleanCoinsSignal cleanCoinsSignal{ get; set; }

		public override void Execute()
		{
			//Clean up the Player
			if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER) != null)
			{
				PlayerView playerView = injectionBinder.GetInstance<PlayerView> (GameElement.PLAYER);
				destroyPlayerSignal.Dispatch (playerView, 0, true);
			}

			cleanCoinsSignal.Dispatch ();

			//Clean up enemies
			EnemyView[] enemies = gameField.GetComponentsInChildren<EnemyView> ();
			foreach (EnemyView enemy in enemies)
			{
				destroyEnemySignal.Dispatch (enemy, 0, false);
			}

			enemyPoolManager.cleanPools ();
		}
	}
}


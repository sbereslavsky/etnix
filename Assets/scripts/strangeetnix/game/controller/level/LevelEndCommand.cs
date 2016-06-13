//Executes when the user clears a level. Increments the game level.

using System;
using strange.extensions.command.impl;

namespace strangeetnix.game
{
	public class LevelEndCommand : Command
	{
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public StopEnemySpawnerSignal stopEnemySpawnerSignal{get;set;}

		public override void Execute ()
		{
			if (gameModel.levelInProgress)
			{
				gameModel.levelInProgress = false;
				//gameModel.level++;
				//updateLevelSignal.Dispatch (gameModel.level);
			}

			stopEnemySpawnerSignal.Dispatch ();
		}
	}
}


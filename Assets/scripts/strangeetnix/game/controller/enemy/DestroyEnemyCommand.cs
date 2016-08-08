//An Enemy can be destroyed in one of three ways:
//1. The player's missile can strike it, earning the player points.
//2. The Enemy can fly offscreen.
//3. The Enemy may simply be cleaned up if left behind at the end of a level.

//We're using pooling, so Enemies are never really "destroyed". We just move
//them offscreen and reset them until we need one again. This is more memory
//and performance friendly than the constant creation/destruction of Objects.

using System;
using System.Collections;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.pool.api;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class DestroyEnemyCommand : Command
	{
		//The pool to which we return the enemies

		//The specific enemy being destroyed
		[Inject]
		public EnemyView enemyView{get;set;}

		[Inject]
		public IEnemyPoolManager enemyPoolManager{ get; set; }

		//True if this destruction earns the player points (False if it flew offscreen
		//or was cleaned up at end of level)

		[Inject]
		public ISpawner enemySpawner{ get; set; }

		[Inject]
		public AddDialogSignal addDialogSignal{ get; set; }

		[Inject]
		public float delayToDestroy{ get; set; }

		[Inject]
		public bool isPointEarning{ get; set; }
		
		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		//Keeper of score, level, etc.
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner{ get; set; }

		private static Vector3 PARKED_POS = new Vector3(1000f, 0f, 1000f);
		
		//An offscreen location to place the recycled Enemies.
		//Arguably this value should be in a config somewhere.
		//private static Vector3 PARKED_POS = new Vector3(1000f, 0f, 1000f);

		//private bool _isRun = true;

		public override void Execute ()
		{
			if (isPointEarning)
			{
				//Give us some points
				//int level = enemyView.level;

				//NOTE: arguably all the point-earning from destroying Rocks and Enemies
				//should be offloaded to a set of ScoreCommands. Certainly in a more complex game,
				//You'd do yourself a favor by centralizing the tabulation of scores.

				//gameModel.score += gameConfig.baseEnemyScore * level;
				gameModel.levelModel.score += 10;
				//updateHudItemSignal.Dispatch (UpdateHudItemType.SCORE, gameModel.levelModel.score);
			}

			//enemyView.gameObject.SetActive (false);
			string enemyName = enemyView.gameObject.name;
			gameModel.levelModel.enemyManager.removeEnemy (enemyName);
			//enemyView.destroyView(delayToDestroy);

			if (delayToDestroy > 0) {
				Retain ();
				routineRunner.StartCoroutine (decreaseCount ());
			} else {
				enemyView.transform.localPosition = PARKED_POS;
				enemyView.gameObject.SetActive (false);
				enemyPoolManager.returnInstance(enemyView.poolKey, enemyView.gameObject);
				enemyView.destroyComponent ();
			}
		}

		IEnumerator decreaseCount()
		{
			yield return new WaitForSeconds (delayToDestroy+0.1f);

			//...and RETURN THEM TO THE POOL!
			if (enemyView != null && enemyView.gameObject != null) {
				enemyView.gameObject.transform.localPosition = PARKED_POS;
				enemyView.gameObject.SetActive (false);
				enemyPoolManager.returnInstance (enemyView.poolKey, enemyView.gameObject);
				enemyView.destroyComponent ();

				onCoroutineComplete ();
			}
			Release ();
		}

		private void onCoroutineComplete()
		{
			if (gameModel != null) {
				gameModel.levelModel.enemyCount--;

				if (enemySpawner != null && enemySpawner.isNobodyToSpawn && gameModel.levelModel.enemyCount == 0) {
					addDialogSignal.Dispatch (DialogType.WIN_GAME);
				}
			}
		}
	}
}


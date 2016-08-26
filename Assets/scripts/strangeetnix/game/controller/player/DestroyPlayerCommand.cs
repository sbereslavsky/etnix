//Destroy the player's ShipView. This can happen for two reasons:
//1. The player got struck by an object (missile, rock, enemy).
//2. The level/game ended, and we're simply cleaning up.

using System;
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class DestroyPlayerCommand : Command
	{
		//Reference to the player's ship
		[Inject]
		public PlayerView playerView{ get; set; }

		[Inject]
		public float delayToDestroy{ get; set; }

		//Boolean to indicate whether this destruction is for cleanup
		[Inject]
		public bool isEndOfLevel{ get; set; }

		//Tracking of lives lost
		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public StopEnemySpawnerSignal stopEnemySpawnerSignal { get; set; }

		[Inject]
		public AddDialogSignal addDialogSignal{ get; set; }

		[Inject]
		public StopEnemySignal stopEnemySignal { get; set; }

		[Inject]
		public CameraEnabledSignal cameraEnabledSignal { get; set; }

		//Need a delay between ship destruction and the next creation. Run a coroutine.
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		public override void Execute ()
		{
			//Gating boolean so we don't double-destroy the player.
			//This can happen because the ShipView is pretty dumb. It simply reports
			//that a collision occurred, so collision with multiple rocks/missiles
			//can over-report destruction.
			//Commands are the brains, so we let this Command (together with the model) decide whether
			//this collision represents a genuine ship destruction.
			if (!gameModel.levelInProgress)
			{
				return;
			}

			//Not isEndOfLevel means the ship was in fact destroyed, not just cleaned up
			if (!isEndOfLevel)
			{
				gameModel.levelInProgress = false;

				playerView.destroyView (delayToDestroy);

				//Are we at the end of the game?
				if (gameModel.playerModel.hp <= 0)
				{
					cameraEnabledSignal.Dispatch (false);
					stopEnemySignal.Dispatch ();
					gameModel.roomModel.enemyManager.stopEnemies ();
					stopEnemySpawnerSignal.Dispatch ();
					//gameModel.playerModel.resetHp();
				}
				if (delayToDestroy > 0) {
					Retain ();
					routineRunner.StartCoroutine (waitToShowLoseDialog ());
				}
			}

			if (!gameModel.isRoomLevel) {
				gameModel.playerPosX = playerView.gameObject.transform.position.x;
			}
			//Unbind the current instance. If we create another ship, the new instance will get
			//re-bound (see CreatePlayerCommand).
			if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER) != null)
				injectionBinder.Unbind<PlayerView> (GameElement.PLAYER);			
		}

		//Wait a couple seconds, then request another player
		private IEnumerator waitToShowLoseDialog()
		{
			yield return new WaitForSeconds (delayToDestroy + 0.1f);
			if (addDialogSignal != null) {
				addDialogSignal.Dispatch (DialogType.LOSE_GAME);
			}

			Release ();
		}
	}
}


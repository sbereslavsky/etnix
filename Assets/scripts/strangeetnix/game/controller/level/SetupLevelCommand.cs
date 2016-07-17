//At the start of each level, place the player and the enemy spawner

using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class SetupLevelCommand : Command
	{
		[Inject]
		public ResetGameCameraSignal resetGameCameraSignal{ get; set; }

		[Inject]
		public CreatePlayerSignal createPlayerSignal{ get; set; }

		[Inject]
		public CreateEnemySpawnerSignal createEnemySpawnerSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		public override void Execute ()
		{
			GameObject gameCamera = GameObject.Find ("gameCamera");
			GameCameraView cameraView = null;
			if (gameCamera) {
				if (gameCamera.GetComponent<GameCameraView> () == null) {
					gameCamera.AddComponent<GameCameraView> ();
				}
				cameraView = gameCamera.GetComponent<GameCameraView> ();
			}

			resetGameCameraSignal.Dispatch (false);
			if (cameraView != null && !cameraView.enabled) {
				cameraView.enabled = true;
			}

			createPlayerSignal.Dispatch (gameModel.playerPosX);

			if (gameModel != null && gameModel.levelModel.hasEnemy) {
				createEnemySpawnerSignal.Dispatch ();
			}
		}
	}
}


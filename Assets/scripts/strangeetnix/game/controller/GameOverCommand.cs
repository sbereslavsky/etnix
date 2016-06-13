using System;
using UnityEngine;
using strange.extensions.command.impl;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class GameOverCommand : Command
	{
		[Inject]
		public SwitchCanvasSignal switchCanvasSignal{ get; set; }

		[Inject]
		public SwitchLevelSignal switchLevelSignal{ get; set; }

		[Inject]
		public LevelEndSignal levelEndSignal{ get; set; }

		[Inject]
		public ResetGameCameraSignal resetGameCameraSignal{ get; set; }

		[Inject]
		public DestroyGameFieldSignal destroyGameFieldSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }
		
		public override void Execute()
		{
			levelEndSignal.Dispatch ();
			destroyGameFieldSignal.Dispatch ();

			if (gameModel.isRoomLevel) {
				gameModel.playerPosX = 0;
				switchLevelSignal.Dispatch (0);
				resetGameCameraSignal.Dispatch (true);
			}

			switchCanvasSignal.Dispatch (UIStates.MAIN);
		}
	}
}


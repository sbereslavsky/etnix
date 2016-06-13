using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class SwitchLevelCommand : Command
	{
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public UpdateGameCanvasSignal updateGameCanvasSignal{ get; set; }

		[Inject]
		public int roomNum { get; set; }

		public override void Execute ()
		{
			gameModel.switchLevel (roomNum);
			gameModel.updateLevelModel (gameConfig);
			updateGameCanvasSignal.Dispatch ();
		}
	}
}


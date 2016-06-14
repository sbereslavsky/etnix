using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class ExitRoomCommand: Command
	{
		[Inject]
		public SwitchLevelSignal switchLevelSignal{get;set;}

		[Inject]
		public LevelStartSignal levelStartSignal{get;set;}

		[Inject]
		public LevelEndSignal levelEndSignal{ get; set; }

		[Inject]
		public ResetGameCameraSignal resetGameCameraSignal{ get; set; }

		[Inject]
		public DestroyGameFieldSignal destroyGameFieldSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		public override void Execute ()
		{
			levelEndSignal.Dispatch ();
			destroyGameFieldSignal.Dispatch ();

			switchLevelSignal.Dispatch (0);
			gameModel.playerModel.resetHp ();
			resetGameCameraSignal.Dispatch (false);

			levelStartSignal.Dispatch ();
		}
	}
}


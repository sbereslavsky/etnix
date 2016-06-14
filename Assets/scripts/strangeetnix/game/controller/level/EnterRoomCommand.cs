using System;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.context.api;

namespace strangeetnix.game
{
	public class EnterRoomCommand : Command
	{
		[Inject]
		public SwitchLevelSignal switchLevelSignal{get;set;}

		[Inject]
		public LevelStartSignal levelStartSignal{get;set;}

		[Inject]
		public LevelEndSignal levelEndSignal{ get; set; }

		[Inject]
		public DestroyGameFieldSignal destroyGameFieldSignal{ get; set; }

		[Inject]
		public CreateEnemyWaveSignal createEnemyWaveSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public int waveId{ get; set; }

		public override void Execute ()
		{
			levelEndSignal.Dispatch ();
			destroyGameFieldSignal.Dispatch ();

			switchLevelSignal.Dispatch (gameModel.roomNum);
			createEnemyWaveSignal.Dispatch (waveId);

			levelStartSignal.Dispatch ();
		}
	}
}


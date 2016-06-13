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
		public UpdateGameCanvasSignal updateGameCanvasSignal{ get; set; }

		[Inject]
		public int roomNum{ get; set; }

		public override void Execute ()
		{
			levelEndSignal.Dispatch ();
			destroyGameFieldSignal.Dispatch ();

			switchLevelSignal.Dispatch (roomNum);

			levelStartSignal.Dispatch ();
		}
	}
}


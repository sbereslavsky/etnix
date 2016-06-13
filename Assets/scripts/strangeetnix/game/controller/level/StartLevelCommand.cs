//Kick off the level

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class StartLevelCommand : Command
	{
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public LevelStartedSignal levelStartedSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal { get; set; }

		[Inject]
		public SetupLevelSignal setupLevelSignal{ get; set; }

		public override void Execute ()
		{
			setupLevelSignal.Dispatch ();
			gameModel.levelInProgress = true;
			levelStartedSignal.Dispatch ();

			updateHudItemSignal.Dispatch (UpdateHudItemType.HP, gameModel.playerModel.hp);
			updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);
			updateHudItemSignal.Dispatch (UpdateHudItemType.COOLDOWN, 0);
		}
	}
}


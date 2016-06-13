//User has asked to Start a game. Let's Rock!

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class StartGameCommand : Command
	{
		[Inject]
		public GameStartedSignal gameStartedSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		//temperory absent start level screen
		[Inject]
		public LevelStartSignal levelStartSignal{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		public override void Execute ()
		{
			//Reset level model
			gameModel.levelModel.Reset ();

			//Update all the model values
			updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);
			updateHudItemSignal.Dispatch (UpdateHudItemType.HP, gameModel.playerModel.hp);
			//updateHudItemSignal.Dispatch (UpdateHudItemType.COOLDOWN, gameModel.playerModel.cooldown);
			//updateHudItemSignal.Dispatch (UpdateHudItemType.SCORE, gameModel.levelModel.score);

			//Tell everyone who cares that we've started
			gameStartedSignal.Dispatch ();

			levelStartSignal.Dispatch ();
		}
	}
}


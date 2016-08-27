using System;
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections;

namespace strangeetnix.game
{
	public class AddExpCommand : Command
	{
		[Inject]
		public int addValue { get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public IGameConfig gameConfig { get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		public override void Execute ()
		{
			gameModel.playerModel.addExp (addValue);
			//gameModel.playerModel.setEndExp ();

			updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);
		}
	}
}


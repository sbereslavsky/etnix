using System;
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections;

namespace strangeetnix.game
{
	public class AddHpCommand : Command
	{
		[Inject]
		public int value { get; set; }

		[Inject]
		public bool isPositive { get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		public override void Execute ()
		{
			IPlayerModel playerModel = gameModel.playerModel;

			if (isPositive) {
				playerModel.addHp (value);
			} else {
				playerModel.decHp (value);
			}

			updateHudItemSignal.Dispatch (UpdateHudItemType.HP, playerModel.hp);
		}
	}
}


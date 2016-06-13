using System;
using UnityEngine;
using strange.extensions.command.impl;

namespace strangeetnix.game
{
	public class PauseGameCommand : Command
	{
		[Inject]
		public bool isPause{ get; set; }

		public override void Execute ()
		{
			Time.timeScale = (isPause) ? 0 : 1;
		}
	}
}


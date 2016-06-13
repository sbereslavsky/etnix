//At the end of the Game, do whatever cleanup is required.

using System;
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections;

namespace strangeetnix.game
{
	public class StopEnemySpawnerCommand : Command
	{
		[Inject]
		public ISpawner spawner{ get; set; }

		public override void Execute ()
		{
			spawner.stop ();
		}
	}
}


//API for a device that spawns things.

using System;

namespace strangeetnix.game
{
	public interface ISpawner
	{
		bool isNobodyToSpawn { get; }

		//Start spawning
		void start();

		//Stop spawning
		void stop();

		//init spawner
		void init ();
	}
}


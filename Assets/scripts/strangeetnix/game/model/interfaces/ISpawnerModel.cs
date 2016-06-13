using System;
using UnityEngine;

namespace strangeetnix.game
{
	public interface ISpawnerModel
	{
		float spawnTime {get;}
		float spawnDelay {get;}

		int count { get;}
		int maxCount { get;}

		GameObject[] items { get; } 

		void Reset();
	}
}

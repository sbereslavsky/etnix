using System;
using UnityEngine;
using System.Collections.Generic;
using strange.extensions.pool.api;

namespace strangeetnix.game
{
	public interface IEnemyPoolManager
	{
		void addPool(ICharAssetVO charAssetVO);
		IPool<GameObject> getPoolByKey(string key);
		void returnInstance (string key, GameObject enemy);
		void cleanPools ();
	}
}


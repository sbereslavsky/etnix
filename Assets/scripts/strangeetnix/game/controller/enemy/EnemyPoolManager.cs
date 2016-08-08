using System;
using UnityEngine;
using System.Collections.Generic;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;

namespace strangeetnix.game
{
	public class EnemyPoolManager : IEnemyPoolManager
	{
		[Inject]
		public IResourceManager resourceManager { get; set; }

		private Dictionary<string, IPool<GameObject>> _poolList;

		public EnemyPoolManager ()
		{
			_poolList = new Dictionary<string, IPool<GameObject>>();
		}

		public void addPool(ICharAssetVO charAssetVO)
		{
			IPool<GameObject> enemyPool = new Pool<GameObject> ();
			enemyPool.instanceProvider = new ResourceInstanceProvider (charAssetVO.assetData, resourceManager);
			enemyPool.inflationType = PoolInflationType.INCREMENT;

			_poolList.Add (charAssetVO.assetData.id, enemyPool);
		}

		public void returnInstance(string key, GameObject enemy)
		{
			IPool<GameObject> enemyPool = getPoolByKey (key);
			if (enemyPool != null) {
				enemyPool.ReturnInstance (enemy);
			} else {
				Debug.LogError ("EnemyPoolManager.returnInstance. Error return instance = "+key);
			}
		}

		public IPool<GameObject> getPoolByKey(string key)
		{
			IPool<GameObject> enemyPool = (_poolList.ContainsKey (key)) ? _poolList [key] : null;
			return enemyPool;
		}

		public void cleanPools()
		{
			Debug.Log ("EnemyPoolManager.clear start");
			foreach (KeyValuePair<string, IPool<GameObject>> pair in _poolList)
			{
				IPool<GameObject> enemyPool = pair.Value;
				enemyPool.Clean ();
			}

			Debug.Log ("EnemyPoolManager.clear end");
		}
	}
}


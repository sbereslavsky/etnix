using UnityEngine;
using System;

namespace strangeetnix.game
{
	public class SpawnerModel : ISpawnerModel 
	{
		private float _spawnTime;
		private float _spawnDelay;

		private int _count;
		private int _maxCount = 3;

		private int _maxItems = 3;
		private GameObject[] _items;

		public SpawnerModel()
		{
			Reset();
		}

		public void Reset()
		{
			_count = 0;
			_items = new GameObject[_maxItems];
		}

		public GameObject[] items
		{
			get { return _items; }
			set { _items = value; }
		} 

		public float spawnTime
		{
			get { return _spawnTime; }
			set { _spawnTime = value; }
		} 

		public float spawnDelay
		{
			get { return _spawnDelay; }
			set { _spawnDelay = value; }
		} 

		public int maxItems
		{
			get { return _maxItems; }
			set { _maxItems = value; }
		} 

		public int maxCount
		{
			get { return _maxCount; }
			set { _maxCount = value; }
		} 

		public int count
		{
			get { return _count; }
		}
	}
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class PlayerTriggerManager
	{
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		public PlayerView playerView { get; set; }

		private Dictionary<string, Collider2D> _list;

		public PlayerTriggerManager ()
		{
			if (_list == null) {
				_list = new Dictionary<string, Collider2D> ();
			} 
		}

		public void reset ()
		{
			if (_list != null && _list.Count > 0) {
				_list.Clear ();
			}
		}

		public void addTrigger(Collider2D collider)
		{
			string key = collider.gameObject.name;
			if (!_list.ContainsKey(key)) {
				_list.Add (key, collider);
			}
		}

		public void removeTriggerByKey(string enemyKey)
		{
			if (_list.ContainsKey(enemyKey)) {
				_list.Remove (enemyKey);
			}
		}

		public List<GameObject> getEnemyToHit()
		{
			List<GameObject> list = new List<GameObject> (10);
			List<Collider2D> values = new List<Collider2D> (_list.Values);
			if (values.Count > 0) {
				foreach(Collider2D c in values)
				{
					if (!isCollisionOut(c) && playerView.gameObject.transform.localScale.x > 0 && c.gameObject.transform.localScale.x > 0 ||
						playerView.gameObject.transform.localScale.x < 0 && c.gameObject.transform.localScale.x < 0) {
						list.Add (c.gameObject);
					}
				}
			}

			return list;
		}

		protected bool isCollisionOut(Collider2D other)
		{
			BoxCollider2D collider2d = playerView.GetComponent<BoxCollider2D> ();
			if (collider2d != null) {
				float dist1 = other.bounds.SqrDistance (collider2d.bounds.center);
				float width = (other as BoxCollider2D).size.x + collider2d.size.x;
				bool result = (width * 0.3f > dist1 || dist1 > width * 1.2f);
			}

			return false;
		}
	}
}


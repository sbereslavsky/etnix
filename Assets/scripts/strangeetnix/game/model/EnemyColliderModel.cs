using System;
using UnityEngine;

namespace strangeetnix.game
{
	public class EnemyColliderModel
	{
		public EnemyView view { get; private set; }
		public String name { get; private set; }

		public EnemyStates state { get; set; }

		public string triggerKeyBefore { get; set; }  
		public string triggerKeyAfter { get; set; }

		public bool isPlayerTrigger { get; set; }

		public EnemyColliderModel (EnemyView view1)
		{
			view = view1;
			name = view.gameObject.name;

			isPlayerTrigger = false;
			triggerKeyBefore = null;
			triggerKeyAfter = null;
		}
	}
}


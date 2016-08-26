using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public class DropCoinView : View
	{
		private const float DELAY_BEFORE_FLY = 1f;

		internal Signal endDelaySignal = new Signal(); 
		internal Signal forceDestroySignal = new Signal(); 

		public int coinValue = 0;

		public void init ()
		{
			StartCoroutine (waitBeforeFly());
		}

		private IEnumerator waitBeforeFly()
		{
			yield return new WaitForSeconds (DELAY_BEFORE_FLY);

			endDelaySignal.Dispatch ();

			StopAllCoroutines ();
		}
	}
}


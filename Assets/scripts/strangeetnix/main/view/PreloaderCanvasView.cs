using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine.SceneManagement;

namespace strangeetnix.main
{
	public class PreloaderCanvasView : View
	{
		private const string LOADING_TEXT = "Loading ";
		public Text loadText;

		internal void updateText (float progress1)
		{
			int sumProgress = (int) progress1*100;
			string loadString = LOADING_TEXT + sumProgress.ToString () + "%";    
			loadText.text = loadString;
		}
	}
}


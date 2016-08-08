using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace strangeetnix.ui
{
	public class PreloaderView : TransformDialogView
	{
		public Text textTitle;

		internal void setText(int value)
		{
			Debug.Log ("PreloaderView.setText - " + value.ToString ());
			textTitle.text = "Loading: " + value.ToString ()+"%";
		}
	}
}


using System;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.ui
{
	public class TransformDialogView : View, ITransformDialogView
	{
		public RectTransform background;

		internal RectTransform rectTransform;

		public void updateBgTransform(RectTransform parentTransform)
		{
			rectTransform = this.GetComponent<RectTransform> ();
			background.sizeDelta = new Vector2 (parentTransform.sizeDelta.x, parentTransform.sizeDelta.y);
		}
	}
}


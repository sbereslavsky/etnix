using System;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.ui
{
	public class TransformDialogView : View, ITransformDialogView
	{
		public RectTransform background;

		public void updateBgTransform(RectTransform parentTransform)
		{
			background.sizeDelta = new Vector2 (parentTransform.sizeDelta.x, parentTransform.sizeDelta.y);
		}
	}
}


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;

using strangeetnix.game;

namespace strangeetnix.ui
{
	public class MainCanvasView : View
	{
		public float OFFSET_X = 360;

		public GameObject getUserDialog(DialogType state, int index=0)
		{
			switch (state) {
			case DialogType.CHAR_INFO:
				destroyPanelByIndex (index);
				return addDialog (AssetConfig.DIALOG_CHAR_INFO, index);

			case DialogType.CHAR_LIST:
				destroyPanelByIndex (index);
				return addDialog (AssetConfig.DIALOG_CHAR_LIST, index);

			case DialogType.CHAR_EDIT:
				return addDialog (AssetConfig.DIALOG_CHAR_EDIT);
			}

			return null;
		}

		private GameObject addDialog(AssetPathData assetData, int index = 0)
		{
			float startX = (index > 0) ? (index-2)*OFFSET_X : 0;
			GameObject panel = (GameObject)Instantiate (Resources.Load (assetData.path), new Vector3(startX , 0, 0), Quaternion.identity);
			panel.name = (index > 0) ? assetData.id + index : assetData.id;
			panel.transform.SetParent (gameObject.transform, false);
			return panel;
		}

		public void destroyDialogCharEdit()
		{
			destroyPanel (AssetConfig.DIALOG_CHAR_EDIT.id);
		}

		public void destroyPanelByIndex(int index)
		{
			destroyPanel (AssetConfig.DIALOG_CHAR_INFO.id + index);
			destroyPanel (AssetConfig.DIALOG_CHAR_LIST.id + index);
		}

		private void destroyPanel(string value)
		{
			GameObject panel = GameObject.Find (value);
			if (panel) {
				Destroy (panel);
			}
		}
	}
}

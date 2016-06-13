using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;

namespace strangeetnix.ui
{
	public class MainCanvasView : View
	{
		public float OFFSET_X = 360;
		private const string DIALOG_CHAR_INFO = "DialogCharInfo";
		private const string DIALOG_CHAR_EDIT = "DialogCharEdit";
		private const string DIALOG_CHAR_LIST = "DialogCharList";

		public GameObject getUserDialog(DialogType state, int index=0)
		{
			switch (state) {
			case DialogType.CHAR_INFO:
				destroyPanelByIndex (index);
				return addDialog (DIALOG_CHAR_INFO, index);

			case DialogType.CHAR_LIST:
				destroyPanelByIndex (index);
				return addDialog (DIALOG_CHAR_LIST, index);

			case DialogType.CHAR_EDIT:
				return addDialog (DIALOG_CHAR_EDIT);
			}

			return null;
		}

		private GameObject addDialog(string dialogId, int index = 0)
		{
			float startX = (index > 0) ? (index-2)*OFFSET_X : 0;
			GameObject panel = (GameObject)Instantiate (Resources.Load ("ui/"+dialogId), new Vector3(startX , 0, 0), Quaternion.identity);
			panel.name = (index > 0) ? dialogId + index : dialogId;
			panel.transform.SetParent (gameObject.transform, false);
			return panel;
		}

		public void destroyDialogCharEdit()
		{
			destroyPanel (DIALOG_CHAR_EDIT);
		}

		public void destroyPanelByIndex(int index)
		{
			destroyPanel (DIALOG_CHAR_INFO + index);
			destroyPanel (DIALOG_CHAR_LIST + index);
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

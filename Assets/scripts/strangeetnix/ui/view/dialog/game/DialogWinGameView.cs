using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogWinGameView : TransformDialogView
	{
		public Text textTitle;
		public Button buttonExit;

		internal Signal closeDialogSignal = new Signal();

		internal void init(ILocalizationConfig localizationConfig)
		{
			textTitle.text = localizationConfig.getTextByKey (LocalizationKeys.MENU_TEXT_WIN);
			string buttonExitText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_EXIT);

			setButtonText(buttonExit, buttonExitText);
			buttonExit.onClick.AddListener (onCloseDialog);
		}

		private void setButtonText(Button button, string textValue)
		{
			Text buttonText = button.GetComponentInChildren<Text> ();
			if (buttonText != null) {
				buttonText.text = textValue;
			} else {
				Debug.LogWarning ("Can't find text field in button "+button.name);
			}
		}

		internal void destroy()
		{
			buttonExit.onClick.RemoveListener (onCloseDialog);
		}

		private void onCloseDialog()
		{
			closeDialogSignal.Dispatch ();
		}
	}
}


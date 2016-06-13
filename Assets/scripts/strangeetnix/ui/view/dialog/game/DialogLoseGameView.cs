using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogLoseGameView : TransformDialogView
	{
		public Text textTitle;
		public Button buttonRestart;
		public Button buttonExit;

		internal Signal restartGameSignal = new Signal();
		internal Signal closeDialogSignal = new Signal();

		internal void init(ILocalizationConfig localizationConfig)
		{
			textTitle.text = localizationConfig.getTextByKey (LocalizationKeys.MENU_TEXT_LOSE);
			string buttonRestartText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_RESTART);
			string buttonExitText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_EXIT);

			setButtonText(buttonRestart, buttonRestartText);
			setButtonText(buttonExit, buttonExitText);

			buttonRestart.onClick.AddListener (onRestartGame);
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
			buttonRestart.onClick.RemoveListener (onRestartGame);
			buttonExit.onClick.RemoveListener (onCloseDialog);
		}

		private void onRestartGame()
		{
			restartGameSignal.Dispatch ();
		}

		private void onCloseDialog()
		{
			closeDialogSignal.Dispatch ();
		}
	}
}


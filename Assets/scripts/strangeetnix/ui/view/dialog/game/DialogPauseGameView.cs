using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogPauseGameView : TransformDialogView
	{
		public Text textTitle;
		public Button buttonContinue;
		public Button buttonVillage;
		public Button buttonMenu;

		internal Signal continueGameSignal = new Signal();
		internal Signal gotoMenuSignal = new Signal();
		internal Signal gotoVillageSignal = new Signal();

		internal void init(ILocalizationConfig localizationConfig)
		{
			string titleText = localizationConfig.getTextByKey (LocalizationKeys.MENU_TEXT_PAUSE_GAME);
			string buttonContinueText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_CONTINUE);
			string buttonMenuText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_MENU);
			string buttonVillageText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_VILLAGE);

			textTitle.text = titleText;
			setButtonText(buttonContinue, buttonContinueText);
			setButtonText(buttonVillage, buttonVillageText);
			setButtonText(buttonMenu, buttonMenuText);

			buttonContinue.onClick.AddListener (onContinueGame);
			buttonMenu.onClick.AddListener (onGotoMenu);
			buttonVillage.onClick.AddListener (onGotoVillage);
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
			buttonContinue.onClick.RemoveListener (onContinueGame);
			buttonMenu.onClick.RemoveListener (onGotoMenu);
			buttonVillage.onClick.RemoveListener (onGotoVillage);
		}

		private void onContinueGame()
		{
			continueGameSignal.Dispatch ();
		}

		private void onGotoMenu()
		{
			gotoMenuSignal.Dispatch ();
		}

		private void onGotoVillage()
		{
			gotoVillageSignal.Dispatch ();
		}
	}
}


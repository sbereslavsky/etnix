using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogChooseWaveView : TransformDialogView
	{
		public Text textTitle;
		public Button[] buttonWaves;

		internal Signal<int> startWaveSignal = new Signal<int>();

		private int _waveCount;

		//private string WAVE_LISTENER = "WaveListener";

		internal void init(ILocalizationConfig localizationConfig, int waveCount)
		{
			_waveCount = waveCount;
			string titleText = localizationConfig.getTextByKey (LocalizationKeys.MENU_TEXT_CHOOSE_WAVE);
			string buttonWaveText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_WAVE);
			textTitle.text = titleText;

			if (buttonWaves != null) {
				Button buttonWave;
				for (byte i = 0; i < buttonWaves.Length; i++) {
					buttonWave = buttonWaves [i];
					if (i < waveCount) {
						int buttonId = i + 1;
						setButtonText(buttonWave, buttonWaveText + buttonId.ToString());
						buttonWave.name = buttonId.ToString();
						buttonWave.onClick.AddListener (() => {OnChooseWave(buttonId);});
					} else {
						buttonWave.gameObject.SetActive (false);
					}
				}
			}
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

		private void OnChooseWave(int waveId)
		{
			startWaveSignal.Dispatch (waveId);
		}

		internal void destroy()
		{
			Button buttonWave;
			for (byte i = 0; i < buttonWaves.Length; i++) {
				buttonWave = buttonWaves [i];
				if (buttonWave.gameObject.activeInHierarchy) {
					//buttonWave.onClick.RemoveListener (WAVE_LISTENER);
				}
			}
		}
	}
}


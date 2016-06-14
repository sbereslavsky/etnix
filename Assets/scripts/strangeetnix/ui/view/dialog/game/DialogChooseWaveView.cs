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
		[Inject]
		public IScreenUtil screenUtil{ get; set; }

		public Text textTitle;
		public Button[] buttonWaves;
		public Button buttonBack;

		internal Signal<int> startWaveSignal = new Signal<int>();
		internal Signal closeDialogSignal = new Signal();

		internal void init(ILocalizationConfig localizationConfig, int waveCount)
		{
			string titleText = localizationConfig.getTextByKey (LocalizationKeys.MENU_TEXT_CHOOSE_WAVE);
			string buttonWaveText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_WAVE);
			string buttonBackText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_EXIT);
			textTitle.text = titleText;
			screenUtil.setButtonText (buttonBack, buttonBackText);
			buttonBack.onClick.AddListener (onClose);

			if (buttonWaves != null) {
				Button buttonWave;
				for (byte i = 0; i < buttonWaves.Length; i++) {
					buttonWave = buttonWaves [i];
					if (i < waveCount) {
						int buttonId = i + 1;
						screenUtil.setButtonText(buttonWave, buttonWaveText + buttonId.ToString());
						buttonWave.name = buttonId.ToString();
						buttonWave.onClick.AddListener (() => {OnChooseWave(buttonId);});
					} else {
						buttonWave.gameObject.SetActive (false);
					}
				}
			}
		}

		private void onClose()
		{
			closeDialogSignal.Dispatch ();
		}

		private void OnChooseWave(int waveId)
		{
			startWaveSignal.Dispatch (waveId);
		}

		//check to removeListener!!!
		internal void destroy()
		{
			buttonBack.onClick.RemoveListener (onClose);

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


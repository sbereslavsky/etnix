using System;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.UI;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class GameCanvasView : View
	{
		[Inject]
		public IScreenUtil screenUtil{ get; set; }

		public GameUIButton buttonLeft;
		public GameUIButton buttonRight;
		public GameUIButton buttonHit;
		public GameUIButton buttonPause;

		public GameUIButton buttonGo;

		public GameUIButton buttonAddHp;

		public Button buttonExit;
		public Button buttonRestart;

		//public Text nameText;
		public Text hpText;
		public Text expText;
		//public Text scoreText;
		public Text levelText;

		public RectTransform playerHpBar;
		public RectTransform playerExpBar;
		//public SpriteRenderer enemyHpBar;

		private const string DIALOG_PAUSE_GAME = "DialogPauseGame";
		private const string DIALOG_WIN_GAME = "DialogWinGame";
		private const string DIALOG_LOSE_GAME = "DialogLoseGame";
		private const string DIALOG_CHOOSE_WAVE = "DialogChooseWave";

		private const string TITLE_EXP = "Exp: ";
		private const string TITLE_HP = "Hp: ";
		private const string TITLE_SCORE = "Score: ";
		private const string TITLE_COOLDOWN = "Cooldown: ";
		private const string TITLE_LEVEL = "Level: ";
		
		private Vector3 _playerHpScale;
		private Vector3 _playerExpScale;
		//private Vector3 _enemyHpScale;

		internal Signal<ButtonType> clickButtonSignal = new Signal<ButtonType>();
		internal Signal pauseButtonSignal = new Signal();
		internal Signal exitButtonSignal = new Signal();
		internal Signal restartButtonSignal = new Signal();
		internal Signal chooseWaveSignal = new Signal();

		internal void init(ILocalizationConfig localizationConfig)
		{
			// Getting the intial scale of the healthbar (whilst the player has full health).
			_playerHpScale = playerHpBar.transform.localScale;
			_playerExpScale = playerHpBar.transform.localScale;
			//_enemyHpScale = enemyHpBar.transform.localScale;

			string buttonText = "";
			buttonText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_HIT);
			screenUtil.setButtonText (buttonHit, buttonText);
			buttonText = localizationConfig.getTextByKey (LocalizationKeys.BUTTON_GO);
			screenUtil.setButtonText (buttonGo, buttonText);
		}

		internal void initButtonsView(bool battleMode)
		{
			buttonHit.gameObject.SetActive (battleMode);
			buttonGo.gameObject.SetActive (false);

			buttonAddHp.gameObject.SetActive (battleMode);
			//buttonPause.gameObject.SetActive (battleMode);

			//GameObject.Find ("itemsImage").SetActive (battleMode);
			//GameObject.Find ("actionButtonsImage").SetActive (battleMode);
		}

		internal void addButtonEvents(bool value)
		{
			if (value) {
				buttonLeft.pointerDownSignal.AddListener (onLeftButtonDown);
				buttonLeft.pointerUpSignal.AddListener (onButtonUp);

				buttonRight.pointerDownSignal.AddListener (onRightButtonDown);
				buttonRight.pointerUpSignal.AddListener (onButtonUp);

				buttonHit.onClick.AddListener (onHitButton);
				buttonGo.onClick.AddListener (onShowChooseWave);

				buttonAddHp.onClick.AddListener (onAddHp);

				buttonPause.onClick.AddListener (onPauseGame);
				buttonExit.onClick.AddListener (onExitGame);
				buttonRestart.onClick.AddListener (onRestartGame);
			} 
			else {
				buttonRight.pointerDownSignal.RemoveListener (onRightButtonDown);
				buttonRight.pointerUpSignal.RemoveListener (onButtonUp);

				buttonLeft.pointerDownSignal.RemoveListener (onLeftButtonDown);
				buttonRight.pointerUpSignal.RemoveListener (onButtonUp);

				buttonHit.onClick.RemoveListener (onHitButton);
				buttonGo.onClick.RemoveListener (onShowChooseWave);

				buttonAddHp.onClick.RemoveListener (onAddHp);

				buttonPause.onClick.RemoveListener (onPauseGame);
				buttonExit.onClick.RemoveListener (onExitGame);
				buttonRestart.onClick.RemoveListener (onRestartGame);
			}
		}

		private void ChangeBarColor (SpriteRenderer bar, Color startColor, Color endColor, int value)
		{
			// Set the health bar's colour to proportion of the way between green and red based on the player's health.
			bar.material.color = Color.Lerp(Color.red, Color.green, 1 - value * 0.01f);
		}


		private void UpdateBarScale (RectTransform bar, Vector3 scale, int value)
		{
			// Set the scale of the health bar to be proportional to the player's health.
			bar.transform.localScale = new Vector3(scale.x * value * 0.01f, scale.y, 1);
		}

		/*internal void SetScore(int value)
		{
			scoreText.text = "";//TITLE_SCORE + value.ToString();
		}*/

		internal void SetCooldown(int value)
		{
			if (value > 0) {
				buttonHit.startFill (value, 0.1f);
			} else {
				buttonHit.restart ();
			}
		}

		internal void SetLevel(int value)
		{
			levelText.text = value.ToString();
		}

		internal void SetPlayerHp(int startHp, int currentHp)
		{
			hpText.text = currentHp + "/" + startHp;

			int hpKoef = 100 * currentHp / startHp;
			UpdateBarScale (playerHpBar, _playerHpScale, hpKoef);
		}

		/*internal void SetEnemyHp(int currentHp, int startHp)
		{
			if (startHp > 0) {
				int hpKoef = 100 * currentHp / startHp;
				UpdateBarScale (enemyHpBar, _enemyHpScale, hpKoef);
			} else {
				Debug.LogError ("updateEnemyHp.startHP=0!");
			}
		}*/

		internal void SetExp(int expStart, int expEnd, int expCurrent)
		{
			expText.text = expCurrent + "/" + expEnd;//TITLE_EXP + currentExp.ToString ();

			expCurrent -= expStart;
			int sumExp = expEnd - expStart;

			int expKoef = 100 * expCurrent / sumExp;
			UpdateBarScale (playerExpBar, _playerExpScale, expKoef);
		}

		internal void showRoomButton (bool value)
		{
			if (buttonGo != null) {
				buttonGo.gameObject.SetActive (value);
			}
		}

		internal GameObject addDialog (string dialogId)
		{
			GameObject dialog = (GameObject)Instantiate (Resources.Load ("ui/"+dialogId), Vector3.zero, Quaternion.identity);
			dialog.name = dialogId;
			dialog.transform.SetParent (gameObject.transform, false);
			return dialog;
		}

		private void onEnterInRoom()
		{
			buttonHit.gameObject.SetActive (false);
		}

		private void onPauseGame()
		{
			pauseButtonSignal.Dispatch ();
		}

		private void onExitGame()
		{
			exitButtonSignal.Dispatch ();
		}

		private void onRestartGame()
		{
			restartButtonSignal.Dispatch ();
		}

		private void onAddHp()
		{
			clickButtonSignal.Dispatch (ButtonType.ADD_HP);
		}

		private void onButtonUp()
		{
			clickButtonSignal.Dispatch (ButtonType.UP);
		}

		private void onLeftButtonDown()
		{
			clickButtonSignal.Dispatch (ButtonType.LEFT_DOWN);
		}

		private void onRightButtonDown()
		{
			clickButtonSignal.Dispatch (ButtonType.RIGHT_DOWN);
		}

		private void onHitButton()
		{
			if (buttonHit.isFilled) {
				clickButtonSignal.Dispatch (ButtonType.HIT);
			}
		}

		private void onShowChooseWave()
		{
			chooseWaveSignal.Dispatch ();
		}
	}
}


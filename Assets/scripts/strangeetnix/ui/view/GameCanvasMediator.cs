﻿using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class GameCanvasMediator : Mediator
	{
		[Inject]
		public GameCanvasView view{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public IResourceManager resourceManager{ get; set; }

		[Inject]
		public GameOverSignal gameOverSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		[Inject]
		public UpdateGameCanvasSignal updateGameCanvasSignal{ get; set; }

		[Inject]
		public AddDialogSignal addDialogSignal{ get; set; }

		[Inject]
		public CloseDialogSignal closeDialogSignal{ get; set; }

		[Inject]
		public ShowRoomButtonSignal showRoomButtonSignal{ get; set; }

		[Inject]
		public AddHpSignal addHpSignal{ get; set; }

		[Inject]
		public GameInputSignal gameInputSignal{ get; set; }

		private int _startPlayerHp = 0;
		private int _expStart = 0;
		private int _expEnd = 0;
		private int _level = 0;

		private int _openDialog = 0;

		private RectTransform _rectTransform;

		private PlayerMediator _playerMediator;

		public override void OnRegister ()
		{
			_startPlayerHp = gameModel.playerModel.hp;
			_expStart = gameModel.playerModel.expStart;
			_expEnd = gameModel.playerModel.expEnd;
			_level = gameModel.playerModel.level;

			_rectTransform = view.gameObject.GetComponent<RectTransform> ();

			view.init (gameConfig.localizationConfig);
			onUpdateCanvas ();
			UpdateListeners (true);

			//view.SetSplayerName (gameModel.playerModel.name);
			updatePlayerHP (_startPlayerHp);
			updatePlayerExp (gameModel.playerModel.exp);
			updateLevel (_level);
			//onScoreUpdate (0);
			//onEnemyHpUpdate (0, 1);

			updatePlayerMediator ();
		}

		public override void OnRemove ()
		{
			UpdateListeners (false);
		}

		private void updatePlayerMediator()
		{
			if (_playerMediator == null) {
				GameObject playerGO = GameObject.FindGameObjectWithTag(PlayerView.ID);
				if (playerGO != null) {
					_playerMediator = playerGO.GetComponent<PlayerMediator> ();
				}
			}
		}

		private void UpdateListeners(bool value)
		{
			view.addButtonEvents (value);

			if (value) {
				//updateScoreSignal.AddListener (onScoreUpdate);
				updateHudItemSignal.AddListener (onUpdateHudItem);
				updateGameCanvasSignal.AddListener (onUpdateCanvas);

				gameInputSignal.AddListener (onGameInput);

				addDialogSignal.AddListener (onAddDialog);
				closeDialogSignal.AddListener (onCloseDialog);
				showRoomButtonSignal.AddListener (onShowRoomButton);

				view.clickButtonSignal.AddListener (onClickButton);
				view.pauseButtonSignal.AddListener (onPauseGame);
				view.chooseWaveSignal.AddListener (onChooseWave);
				view.weaponEditSignal.AddListener (onWeaponEdit);
			} else {
				//updateScoreSignal.RemoveListener (onScoreUpdate);
				updateHudItemSignal.RemoveListener (onUpdateHudItem);
				updateGameCanvasSignal.RemoveListener (onUpdateCanvas);

				if (gameInputSignal != null) {
					gameInputSignal.RemoveListener (onGameInput);
				}

				addDialogSignal.RemoveListener (onAddDialog);
				closeDialogSignal.RemoveListener (onCloseDialog);
				showRoomButtonSignal.RemoveListener (onShowRoomButton);

				view.clickButtonSignal.RemoveListener (onClickButton);
				view.pauseButtonSignal.RemoveListener (onPauseGame);
				view.chooseWaveSignal.RemoveListener (onChooseWave);
				view.weaponEditSignal.RemoveListener (onWeaponEdit);
			}
		}

		//Receive a signal updating GameInput
		private void onGameInput(int input)
		{
			if ((input & GameInputEvent.QUIT) > 0) {
				onAddDialog (DialogType.PAUSE_GAME);
			}
		}

		private void onCloseDialog()//DialogType type)
		{
			if (_openDialog > 0) {
				_openDialog--;
			}
		}

		private void onAddDialog(DialogType type)
		{
			if (_openDialog > 0) {
				return;
			}

			AssetPathData dialogData = null;

			switch (type) {
			case DialogType.WIN_GAME:
				dialogData = AssetConfig.DIALOG_WIN_GAME;
				break;
			case DialogType.PAUSE_GAME:
				dialogData = AssetConfig.DIALOG_PAUSE_GAME;
				break;
			case DialogType.LOSE_GAME:
				dialogData = AssetConfig.DIALOG_LOSE_GAME;
				break;
			case DialogType.CHOOSE_WAVE:
				dialogData = AssetConfig.DIALOG_CHOOSE_WAVE;
				break;
			case DialogType.WEAPON_EDIT:
				dialogData = AssetConfig.DIALOG_WEAPON_EDIT;
				break;
			}

			if (dialogData != null) {				
				GameObject dialogStyle = resourceManager.getResourceByAssetData (dialogData);
				GameObject dialogGO = view.addDialog (dialogStyle, dialogData.id);
				ITransformDialogView dialogView = dialogGO.GetComponent<TransformDialogView> ();
				dialogView.updateBgTransform(_rectTransform);
				_openDialog++;
			}
		}

		private void onShowRoomButton(bool value, int roomNum)
		{
			gameModel.roomNum = roomNum;
			view.showRoomButton (value);
		}

		private void onPauseGame()
		{
			onAddDialog (DialogType.PAUSE_GAME);
		}

		private void onWeaponEdit()
		{
			onAddDialog (DialogType.WEAPON_EDIT);
		}

		private void onChooseWave()
		{
			onAddDialog (DialogType.CHOOSE_WAVE);
		}

		private void onExitButton()
		{
			onAddDialog (DialogType.LOSE_GAME);
		}

		private void onRestartGame()
		{
			onAddDialog (DialogType.LOSE_GAME);
		}

		private void onClickButton(ButtonType type)
		{
			updatePlayerMediator ();
			if (_playerMediator) {
				switch (type)
				{
					case ButtonType.ADD_HP:
						addHpSignal.Dispatch (50, true);
						break;

					case ButtonType.UP:
						_playerMediator.stopWalk (false);
						break;
					case ButtonType.LEFT_DOWN:
						_playerMediator.startLeftWalk (true);
						break;

					case ButtonType.RIGHT_DOWN:
						_playerMediator.startRightWalk (true);
						break;

					case ButtonType.HIT_DOWN:
						if (!_playerMediator.isCycleHit) {
							_playerMediator.isCycleHit = true;
						}
						_playerMediator.startHit ();
						break;

					case ButtonType.HIT_UP:
						_playerMediator.isCycleHit = false;
						break;
				}
			}
		}

		private void onUpdateCanvas()
		{
			view.initButtonsView (gameModel.roomModel.hasEnemy);
			updatePlayerMediator ();
		}

		private void onUpdateHudItem(UpdateHudItemType type, int value)
		{
			switch (type) {
			case UpdateHudItemType.COOLDOWN:
				view.SetCooldown (value);
				break;
			case UpdateHudItemType.LEVEL:
				updateLevel (value);
				break;
			case UpdateHudItemType.HP:
				updatePlayerHP (value);
				break;
			case UpdateHudItemType.EXP:
				updatePlayerExp (value);
				break;
			}
		}

		private void updateLevel(int value)
		{
			view.SetLevel (value);
		}

		private void updatePlayerHP(int currentHP)
		{
			if (_startPlayerHp != gameModel.playerModel.startHp) {
				_startPlayerHp = gameModel.playerModel.startHp;
			}
			view.SetPlayerHp (_startPlayerHp, currentHP);
		}

		private void updatePlayerExp(int currentExp)
		{
			if (gameModel.playerModel.levelUp) {
				gameModel.playerModel.setLevelDataVO ();

				_expStart = gameModel.playerModel.expStart;
				_expEnd = gameModel.playerModel.expEnd;
				_level = gameModel.playerModel.level;

				_startPlayerHp = gameModel.playerModel.hp;
				updatePlayerHP (gameModel.playerModel.hp);
				updateLevel (_level);
			}

			view.SetExp (_expStart, _expEnd, currentExp);
		}
	}
}


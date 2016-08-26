using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogWeaponEditMediator : Mediator
	{
		[Inject]
		public DialogWeaponEditView view{ get; set;}

		[Inject]
		public IGameModel gameModel{ get; set;}

		[Inject]
		public IGameConfig gameConfig{ get; set;}

		[Inject]
		public PauseGameSignal pauseGameSignal { get; set; }

		[Inject]
		public CloseDialogSignal closeDialogSignal{ get; set; }

		[Inject]
		public UpdatePlayerInfoSignal updatePlayerInfoSignal{ get; set; }

		private IPlayerModel _playerModel;

		private bool _isChange = false;

		public override void OnRegister()
		{
			pauseGameSignal.Dispatch (true);

			_playerModel = gameModel.playerModel;

			view.init (_playerModel.userCharVO, gameConfig);
			view.updatePlayerInfo (_playerModel);

			view.updateGoldValue (_playerModel.coins);

			UpdateListeners(true);
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.changeWeaponSignal.AddListener (onChangeWeapon);
				view.changeItem2Signal.AddListener (onChangeItem2);
				view.changeItem3Signal.AddListener (onChangeItem3);
				view.changeEquipedSignal.AddListener (onChangeEquiped);
				view.closeDialogSignal.AddListener (onCloseDialog);
			} else {
				view.changeWeaponSignal.RemoveListener (onChangeWeapon);
				view.changeItem2Signal.RemoveListener (onChangeItem2);
				view.changeItem3Signal.RemoveListener (onChangeItem3);
				view.changeEquipedSignal.RemoveListener (onChangeEquiped);
				view.closeDialogSignal.RemoveListener (onCloseDialog);
			}
		}

		private void onChangeWeapon(int weaponId)
		{
			_playerModel.userCharVO.weaponId = weaponId;
			_playerModel.updateWeaponVO ();

			view.updatePlayerInfo (_playerModel);

			_isChange = true;
		}

		private void onChangeItem2(int itemId2)
		{
			_playerModel.userCharVO.itemId2 = itemId2;
			_playerModel.updateItem2VO ();

			_isChange = true;
		}

		private void onChangeItem3(int itemId3)
		{
			_playerModel.userCharVO.itemId3 = itemId3;
			_playerModel.updateItem3VO ();

			_isChange = true;
		}

		private void onChangeEquiped(int equipedId)
		{
			_playerModel.userCharVO.equipedId = equipedId;
			_playerModel.updateEquipedVO ();

			_isChange = true;
		}

		private void onCloseDialog()
		{
			if (_isChange) {
				gameConfig.save ();
			}

			updatePlayerInfoSignal.Dispatch ();

			view.removeEventListeners ();
			view.clearDropDowns ();

			closeDialogSignal.Dispatch ();
			pauseGameSignal.Dispatch (false);
			Destroy (view.gameObject);
		}
	}
}


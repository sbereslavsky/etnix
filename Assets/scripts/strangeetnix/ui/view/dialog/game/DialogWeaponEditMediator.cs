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

		private IUserCharVO _userCharVO;
		private IUserCharInfoVO _userCharInfoVO;

		public override void OnRegister()
		{
			pauseGameSignal.Dispatch (true);

			int charId = gameModel.playerId;
			_userCharVO = gameConfig.userCharConfig.getUserCharVOById(charId);
			view.init (_userCharVO, gameConfig);
			updateCharInfo();

			view.updateGoldValue (gameModel.playerModel.coins);

			UpdateListeners(true);
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.closeDialogSignal.AddListener (onCloseDialog);
			} else {
				view.closeDialogSignal.RemoveListener (onCloseDialog);
			}
		}

		private void updateCharInfo()
		{
			_userCharInfoVO = _userCharVO.getUserCharInfoVO (gameConfig);
		}

		private void onCloseDialog()
		{
			string value = view.getDropDownText (view.dropDownWeapon);
			_userCharVO.weaponId = gameConfig.weaponConfig.getIdByInfo (value);

			value = view.getDropDownText (view.dropDownEquiped);
			_userCharVO.equipedId = gameConfig.equipedConfig.getIdByInfo (value);

			value = view.getDropDownText (view.dropDownItem2);
			_userCharVO.itemId2 = gameConfig.itemConfig.getIdByInfo (value);

			value = view.getDropDownText (view.dropDownItem3);
			_userCharVO.itemId3 = gameConfig.itemConfig.getIdByInfo (value);

			view.clearDropDowns ();

			gameConfig.save ();
			gameModel.updatePlayerModel (gameConfig);
			updatePlayerInfoSignal.Dispatch ();

			closeDialogSignal.Dispatch ();
			pauseGameSignal.Dispatch (false);
			Destroy (view.gameObject);
		}
	}
}


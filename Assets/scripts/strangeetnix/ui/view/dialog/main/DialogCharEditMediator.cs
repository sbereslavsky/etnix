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
	public class DialogCharEditMediator : Mediator
	{
		[Inject]
		public DialogCharEditView view{ get; set;}

		[Inject]
		public IGameConfig gameConfig{ get; set;}

		[Inject]
		public CloseEditPanelSignal closeEditPanelSignal{ get; set;}

		private IUserCharVO _userCharVO;

		public override void OnRegister()
		{
			int charId = view.charId;
			_userCharVO = gameConfig.userCharConfig.getUserCharVOById(charId);
			view.init (_userCharVO, gameConfig);

			updateCharInfo ();

			UpdateListeners(true);
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.addExpSignal.AddListener (onAddExp);
				view.closeDialogSignal.AddListener (onCloseDialog);
			} else {
				view.addExpSignal.RemoveListener (onAddExp);
				view.closeDialogSignal.RemoveListener (onCloseDialog);
			}
		}

		private void onAddExp(int value)
		{
			_userCharVO.exp = Mathf.Max(0, _userCharVO.exp + value);
			updateCharInfo ();
		}

		private void updateCharInfo()
		{
			IUserCharInfoVO userCharInfoVO = _userCharVO.getUserCharInfoVO (gameConfig);
			view.updateInfo (userCharInfoVO);
		}

		private void onCloseDialog(bool isSave)
		{
			if (isSave) {
				gameConfig.save ();
			}

			closeEditPanelSignal.Dispatch ();
		}
	}
}
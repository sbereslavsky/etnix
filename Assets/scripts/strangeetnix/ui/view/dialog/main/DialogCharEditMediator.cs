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
		private IUserCharInfoVO _userCharInfoVO;

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
				view.closeDialogSignal.AddListener (onCloseDialog);
				view.addExpSignal.AddListener (onAddExp);
			} else {
				view.closeDialogSignal.RemoveListener (onCloseDialog);
				view.addExpSignal.RemoveListener (onAddExp);
			}
		}

		private void updateCharInfo()
		{
			_userCharInfoVO = _userCharVO.getUserCharInfoVO (gameConfig);
			view.updateInfo (_userCharInfoVO);
		}

		private void onAddExp(int value)
		{
			_userCharVO.exp = Mathf.Max(0, _userCharVO.exp + value);
			updateCharInfo ();
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
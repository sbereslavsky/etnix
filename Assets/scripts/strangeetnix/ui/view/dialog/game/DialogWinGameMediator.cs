using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogWinGameMediator : Mediator
	{
		[Inject]
		public DialogWinGameView view { get; set;}

		[Inject]
		public IGameConfig gameConfig { get; set;}

		[Inject]
		public IGameModel gameModel { get; set;}

		[Inject]
		public CloseDialogSignal closeDialogSignal{ get; set; }

		[Inject]
		public GameOverSignal gameOverSignal{ get; set; }

		[Inject]
		public ExitRoomSignal exitRoomSignal{ get; set; }

		[Inject]
		public CleanCoinsSignal cleanCoinsSignal{ get; set; }

		private string _viewName;

		public override void OnRegister()
		{
			_viewName = view.name;
			view.init (gameConfig.localizationConfig);

			UpdateListeners(true);

			Debug.Log ("Add: "+_viewName);
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
			Debug.Log ("Remove: "+_viewName);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.closeDialogSignal.AddListener (onCloseDialog);
			} else {
				view.closeDialogSignal.RemoveListener (onCloseDialog);
			}
		}

		private void onCloseDialog()
		{
			cleanCoinsSignal.Dispatch ();

			gameModel.playerModel.updateUserConfig ();
			gameConfig.save ();

			Destroy (view.gameObject);
			closeDialogSignal.Dispatch ();
			if (gameModel.isRoomLevel) {
				exitRoomSignal.Dispatch ();
			} else {
				gameOverSignal.Dispatch ();
			}
		}
	}
}


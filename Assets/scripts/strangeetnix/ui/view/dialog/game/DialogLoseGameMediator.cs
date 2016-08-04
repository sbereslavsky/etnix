using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogLoseGameMediator : Mediator
	{
		[Inject]
		public DialogLoseGameView view { get; set;}

		[Inject]
		public IGameConfig gameConfig { get; set;}

		[Inject]
		public CloseDialogSignal closeDialogSignal{ get; set; }

		[Inject]
		public RestartGameSignal restartGameSignal{ get; set; }

		[Inject]
		public GameOverSignal gameOverSignal{ get; set; }

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
				view.restartGameSignal.AddListener (onRestartGame);
			} else {
				view.closeDialogSignal.RemoveListener (onCloseDialog);
				view.restartGameSignal.RemoveListener (onRestartGame);
			}
		}

		private void onRestartGame()
		{
			Destroy (view.gameObject);
			restartGameSignal.Dispatch ();
		}

		private void onCloseDialog()
		{
			closeDialogSignal.Dispatch ();
			Destroy (view.gameObject);
			gameOverSignal.Dispatch ();
		}
	}
}


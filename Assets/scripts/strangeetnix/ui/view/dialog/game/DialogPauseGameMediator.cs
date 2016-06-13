using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogPauseGameMediator : Mediator
	{
		[Inject]
		public DialogPauseGameView view { get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IGameConfig gameConfig { get; set; }

		[Inject]
		public ExitRoomSignal exitRoomSignal{ get; set; }

		[Inject]
		public GameOverSignal gameOverSignal{ get; set; }

		[Inject]
		public PauseGameSignal pauseGameSignal { get; set; }

		private string _viewName;

		public override void OnRegister()
		{
			pauseGameSignal.Dispatch (true);

			_viewName = view.name;
			view.init (gameConfig.localizationConfig);

			view.buttonVillage.gameObject.SetActive (gameModel.isRoomLevel);

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
				view.continueGameSignal.AddListener (onContinueGame);
				view.gotoMenuSignal.AddListener (onGotoMenu);
				view.gotoVillageSignal.AddListener (onGotoVillage);
			} else {
				view.continueGameSignal.RemoveListener (onContinueGame);
				view.gotoMenuSignal.RemoveListener (onGotoMenu);
				view.gotoVillageSignal.RemoveListener (onGotoVillage);
			}
		}

		private void onContinueGame()
		{
			destroyView ();
		}

		private void onGotoMenu()
		{
			destroyView ();
			gameOverSignal.Dispatch ();
		}

		private void onGotoVillage()
		{
			destroyView ();
			exitRoomSignal.Dispatch ();
		}

		private void destroyView()
		{
			pauseGameSignal.Dispatch (false);
			Destroy (view.gameObject);
		}
	}
}


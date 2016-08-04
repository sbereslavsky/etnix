using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogChooseWaveMediator : Mediator
	{
		[Inject]
		public DialogChooseWaveView view { get; set;}

		[Inject]
		public CloseDialogSignal closeDialogSignal{ get; set; }

		[Inject]
		public IGameConfig gameConfig { get; set;}

		[Inject]
		public IGameModel gameModel { get; set;}

		[Inject]
		public EnterRoomSignal enterRoomSignal{ get; set; }

		private string _viewName;

		public override void OnRegister()
		{
			_viewName = view.name;
			view.init (gameConfig.localizationConfig, gameConfig.waveConfig.count);

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
				view.startWaveSignal.AddListener (onChooseWave);
				view.closeDialogSignal.AddListener (onCloseDialog);
			} else {
				view.startWaveSignal.RemoveListener (onChooseWave);
				view.closeDialogSignal.RemoveListener (onCloseDialog);
			}
		}

		private void onCloseDialog()
		{
			closeDialogSignal.Dispatch ();
			view.destroy ();
			Destroy (view.gameObject);
		}

		private void onChooseWave(int waveId)
		{
			enterRoomSignal.Dispatch (waveId);
			onCloseDialog ();
		}
	}
}


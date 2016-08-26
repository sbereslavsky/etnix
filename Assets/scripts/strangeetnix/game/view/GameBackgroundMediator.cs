using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class GameBackgroundMediator : Mediator
	{
		//View
		[Inject]
		public GameBackgroundView view { get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public ShowRoomButtonSignal showRoomButtonSignal{ get; set; }

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			view.init(gameModel.roomModel.bgAssetInfo);
			view.showButtonEnter.AddListener (onShowButton);
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			view.showButtonEnter.RemoveListener (onShowButton);
		}

		private void onShowButton(bool value, int roomNum)
		{
			showRoomButtonSignal.Dispatch (value, roomNum);
		}
	}
}


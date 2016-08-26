using System;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class GameFieldMediator : Mediator
	{
		//View
		[Inject]
		public GameFieldView view { get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		private GameObject _bgGO;

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			initBackground (gameModel.roomModel.bgAssetInfo);
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			destroyBackground ();
		}

		private void destroyBackground()
		{
			if (_bgGO != null) {
				Destroy (_bgGO.gameObject);
				_bgGO = null;
			}
		}

		private void initBackground(IAssetVO bgAssetVO)
		{
			GameObject bgStyle = Resources.Load<GameObject> (bgAssetVO.assetData.path);
			_bgGO = (GameObject)Instantiate (bgStyle, Vector3.zero, Quaternion.identity);
			_bgGO.name = name;
			_bgGO.transform.SetParent (view.gameObject.transform, false);

			if (gameModel.isRoomLevel) {
				Vector3 bgPos = _bgGO.gameObject.transform.localPosition;
				bgPos.x = gameModel.playerPosX;
				_bgGO.gameObject.transform.localPosition = bgPos;
			}
		}
	}
}


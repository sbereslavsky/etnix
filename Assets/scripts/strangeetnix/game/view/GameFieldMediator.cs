using System;
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
			addBackground (gameModel.levelModel.bgAssetInfo);
			if (gameModel.isRoomLevel) {
				Vector3 bgPos = _bgGO.gameObject.transform.localPosition;
				bgPos.x = gameModel.playerPosX;
				_bgGO.gameObject.transform.localPosition = bgPos;
			}
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			destroyBackground ();

			//gameInputSignal.RemoveListener (onGameInput);
		}

		private void destroyBackground()
		{
			if (_bgGO != null) {
				Destroy (_bgGO);
			}
		}

		private void addBackground(IAssetVO bgAssetVO)
		{
			_bgGO = createObject (bgAssetVO.name, bgAssetVO.path, Vector3.zero);
		}

		private GameObject createObject(string name, string path, Vector3 position)
		{
			GameObject obj = (GameObject)Instantiate (Resources.Load (path), position, Quaternion.identity);
			obj.name = name;
			obj.transform.SetParent (view.gameObject.transform, false);
			return obj;
		}
	}
}


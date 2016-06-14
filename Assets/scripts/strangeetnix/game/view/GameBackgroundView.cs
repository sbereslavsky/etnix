using System;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class GameBackgroundView : View
	{
		//private float _minX;
		//private float _maxX;

		private Transform _cameraTransform;		// Reference to the camera's transform.

		//private float _width;

		public float[] steps;
		public Transform[] layers;

		public BoxCollider2D wallLeft;
		public BoxCollider2D wallRight;

		//private float _oldPos;

		private Transform _roomCheck;
		private string IN_ROOM 	= "inRoom";
		private int _roomNum;

		private bool _isActiveButton = false;

		internal Signal<bool, int> showButtonEnter = new Signal<bool, int>();

		internal void init (IBgAssetVO bgAssetInfo)
		{
			_roomCheck = (transform.Find(IN_ROOM) != null) ? transform.Find(IN_ROOM).transform : null;
			//_width = bgAssetInfo.width;
			_cameraTransform = Camera.main.gameObject.transform;
			//_oldPos = _cameraTransform.position.x;
			_roomNum = 1;

			if (layers.Length == 0 || layers.Length != steps.Length) {
				this.enabled = false;
				return;
			}

			_isActiveButton = false;

			//_minX = bgAssetInfo.minXAndY.x;
			//_maxX = bgAssetInfo.maxXAndY.x;

			//wallLeft.offset = new Vector2(_minX-9, wallLeft.offset.y);
			//wallRight.offset = new Vector2(_maxX - 7, wallLeft.offset.y);
		}

		void FixedUpdate ()
		{
			//need to update vs oldPos from village to church
			if (_cameraTransform != null) {// && _oldPos != _cameraTransform.position.x) {
				//_oldPos = _cameraTransform.position.x;
				for (byte i = 0; i < layers.Length; i++) {
					updateLayerPosByStep (layers[i], steps[i]);
				}
			}

			if (_roomCheck != null) {
				// Create an array of all the colliders in front of the enemy.
				Collider2D[] frontHits = Physics2D.OverlapPointAll (_roomCheck.position);

				bool isPlayer = false;
				// Check each of the colliders.
				foreach (Collider2D c in frontHits) {
					// If any of the colliders is an Obstacle...
					isPlayer = c.tag.Contains (PlayerView.ID);
					if (isPlayer && !_isActiveButton) {
						_isActiveButton = true;
						showButtonEnter.Dispatch (true, _roomNum);
						return;
					}
				}

				if (!isPlayer && _isActiveButton) {
					_isActiveButton = false;
					showButtonEnter.Dispatch (false, _roomNum);
				}
			}
		}

		private void updateLayerPosByStep(Transform renderer, float step)
		{
			Vector3 layerPos = renderer.position;
			layerPos.x = -_cameraTransform.position.x * step;
			renderer.position = layerPos;
			//renderer.material.mainTextureOffset = new Vector2 (-_cameraTransform.position.x * step, 0);
		}
	}
}


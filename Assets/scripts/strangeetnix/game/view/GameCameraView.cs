using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public class GameCameraView : View 
	{
		public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
		public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
		public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
		public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
		public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
		public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

		private Transform _playerTransform;		// Reference to the player's transform.
		private float _playerPosX = 0;

		public void resetCamera(float playerPosX)
		{
			_playerPosX = playerPosX;
			updateObjects ();
			TrackPlayer(false);
		}

		public void updateObjects()
		{
			GameObject playerGO = GameObject.FindGameObjectWithTag (PlayerView.ID);
			if (playerGO == null) {
				_playerTransform = null;
			}
			else if (_playerTransform == null) {
				_playerTransform = playerGO.transform;
			}
		}

		bool CheckXMargin()
		{
			// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
			float playerPositionX = (_playerTransform != null) ? _playerTransform.position.x : 0;
			return Mathf.Abs(transform.position.x - playerPositionX) > xMargin;
		}

		bool CheckYMargin()
		{
			// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
			float playerPositionY = (_playerTransform != null) ? _playerTransform.position.y : 0;
			return Mathf.Abs(transform.position.y - playerPositionY) > yMargin;
		}

		void FixedUpdate ()
		{
			updateObjects ();
			TrackPlayer();
		}

		void TrackPlayer (bool withSmooth=true)
		{
			// By default the target x and y coordinates of the camera are it's current x and y coordinates.
			float targetX = transform.position.x;
			float targetY = transform.position.y;

			float playerPositionX = _playerPosX;
			if (_playerTransform != null) {
				playerPositionX = _playerTransform.position.x;
				_playerPosX = playerPositionX;
			} else if (!withSmooth) {
				targetX = playerPositionX;
			}

			// If the player has moved beyond the x margin...
			if (withSmooth && CheckXMargin ()) {
				// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
				targetX = Mathf.Lerp (targetX, playerPositionX, xSmooth * Time.deltaTime);
			}

			// If the player has moved beyond the y margin...
			/*if (withSmooth && CheckYMargin ()) {
				// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
				targetY = Mathf.Lerp (targetY, playerPositionY, ySmooth * Time.deltaTime);
			}*/

			// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
			targetX = Mathf.Clamp (targetX, minXAndY.x, maxXAndY.x);
			targetY = Mathf.Clamp (targetY, minXAndY.y, maxXAndY.y);

			// Set the camera's position to the target position with the same z component.
			transform.position = new Vector3(targetX, targetY, transform.position.z);
		}
	}
}
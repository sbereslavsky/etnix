﻿//Utility class providing Camera/GameObject mapping capabilities

using System;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.injector.api;

namespace strangeetnix
{
	//Anchors to the corners/edges/center of screen
	public enum ScreenAnchor
	{
		LEFT,
		RIGHT,
		TOP,
		BOTTOM,
		CENTER_VERTICAL,
		CENTER_HORIZONTAL,
	}

	//AN EXAMPLE OF IMPLICIT BINDINGS
	//You'll note that there is no binding of IScreenUtil to ScreenUtil in any of the Contexts.
	//It's handled automatically here. By default, implicit bindings are single-Context. See
	//RoutineRunner for an example of a Cross-Context implicit binding.
	[Implements(typeof(IScreenUtil))]
	public class ScreenUtil : IScreenUtil
	{
		//The camera in use by the Context
		[Inject(StrangeEtnixElement.GAME_CAMERA)]
		public Camera gameCamera{ get; set; }

		//Get a rect that represents the provided values as a percentage of the screen
		//GameDebugView uses this to create resolution-independent positions for GUI elements
		public Rect GetScreenRect (float x, float y, float width, float height)
		{
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
			return new Rect (x * screenWidth,
				y * screenHeight,
				width * screenWidth,
				height * screenHeight);
		}

		//A method for determining if a gameObject is visible to the camera.
		//I started using renderer.isVisible...but that failed to function correctly in at least one case.
		public bool IsInCamera(GameObject go)
		{
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes (gameCamera);
			return GeometryUtility.TestPlanesAABB (planes, go.GetComponent<Renderer>().bounds);
		}

		//When a rock or the player exists the screen,
		//This "wraps" the GameObject to the far side of the screen
		public void TranslateToFarSide(GameObject go)
		{
			Vector3 pos = go.transform.localPosition;
			Vector3 viewPos = gameCamera.WorldToViewportPoint (pos);

			if (viewPos.x > 1f)
			{
				viewPos.x = .001f;
			}
			else if (viewPos.x < 0f)
			{
				viewPos.x = .999f;
			}

			if (viewPos.y > 1f)
			{
				viewPos.y = .001f;
			}
			else if (viewPos.y < 0f)
			{
				viewPos.y = .999f;
			}

			Vector3 newPos = gameCamera.ViewportToWorldPoint (viewPos);
			go.transform.localPosition = newPos;
		}

		//Calculates entry positions for the enemy
		public float RandomPositionX(float minX, float maxX)
		{
			int side = 1;
			float gameCameraX = gameCamera.transform.position.x;
			if (gameCameraX > minX && gameCameraX < maxX) {
				side = (UnityEngine.Random.Range (1, 1000) % 2 == 0) ? 1 : -1;
			} else if (gameCameraX < minX) {
				side = 1;
			} else if (gameCameraX > maxX) {
				side = -1;
			}

			float halfWidth = gameCamera.orthographicSize * gameCamera.aspect * side;
			return gameCamera.transform.localPosition.x + halfWidth;
		}

		public float cameraAspectRatio 
		{ 
			get { return gameCamera.aspect;} 
		} 

		//Return a Vector3 placing a UI element at some anchored place onscreen
		public Vector3 GetAnchorPosition(ScreenAnchor horizontal, ScreenAnchor vertical)
		{
			float x;
			float y;

			switch (horizontal)
			{
			case ScreenAnchor.LEFT:
				x = 0;
				break;
			case ScreenAnchor.CENTER_HORIZONTAL:
				x = .5f;
				break;
			case ScreenAnchor.RIGHT:
				x = 1f;
				break;
			default:
				throw new Exception ("ScreenUtil.GetAnchorPosition illegal horizontal value");
			}

			switch (vertical)
			{
			case ScreenAnchor.BOTTOM:
				y = 0;
				break;
			case ScreenAnchor.CENTER_VERTICAL:
				y = .5f;
				break;
			case ScreenAnchor.TOP:
				y = 1f;
				break;
			default:
				throw new Exception ("ScreenUtil.GetAnchorPosition illegal horizontal value");
			}
			Vector3 retv = new Vector3 (x, y, gameCamera.transform.localPosition.y);
			retv = gameCamera.ViewportToWorldPoint (retv);
			return retv;
		}

		public void setButtonText(Button button, string textValue)
		{
			Text buttonText = button.GetComponentInChildren<Text> ();
			if (buttonText != null) {
				buttonText.text = textValue;
			} else {
				Debug.LogWarning ("Can't find text field in button "+button.name);
			}
		}
	}
}

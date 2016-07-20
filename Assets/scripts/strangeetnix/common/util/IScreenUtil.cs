//Interface for a service which provides camera utilities
//(see ScreenUtil)

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using strangeetnix.game;

namespace strangeetnix
{
	public interface IScreenUtil
	{
		Rect GetScreenRect(float x, float y, float width, float height);

		bool IsInCamera(GameObject go);

		void TranslateToFarSide(GameObject go);

		SpawnPosition getSpawnPosition (float minX, float maxX, bool canSpawnLeft, bool canSpawnRight);

		float cameraAspectRatio { get; }

		Vector3 GetAnchorPosition(ScreenAnchor horizontal, ScreenAnchor vertical);

		void setButtonText (Button button, string textValue);

		bool isEqualsScaleX (GameObject go1, GameObject go2);

		bool isCollisionOut (BoxCollider2D collider1, BoxCollider2D collider2, float minKoef, float maxKoef);
	}
}
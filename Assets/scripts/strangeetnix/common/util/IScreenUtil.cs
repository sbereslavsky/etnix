//Interface for a service which provides camera utilities
//(see ScreenUtil)

using System;
using UnityEngine;

namespace strangeetnix
{
	public interface IScreenUtil
	{
		Rect GetScreenRect(float x, float y, float width, float height);

		bool IsInCamera(GameObject go);

		void TranslateToFarSide(GameObject go);

		float RandomPositionX (float minX, float maxX);

		float cameraAspectRatio { get; }

		Vector3 GetAnchorPosition(ScreenAnchor horizontal, ScreenAnchor vertical);
	}
}
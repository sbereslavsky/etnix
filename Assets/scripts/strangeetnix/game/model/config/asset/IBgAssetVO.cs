using System;
using UnityEngine;

namespace strangeetnix.game
{
	public interface IBgAssetVO : IAssetVO
	{
		float startPosY { get; set; }

		Vector2 minXAndY { get; set; }
		Vector2 maxXAndY { get; set; }
		Vector2 margin { get; set; }
		Vector2 smooth { get; set; }

		float width { get; set; }

		IBgAssetVO clone();
	}
}


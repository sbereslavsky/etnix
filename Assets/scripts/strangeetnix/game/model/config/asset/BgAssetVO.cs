using System;
using UnityEngine;

namespace strangeetnix.game
{
	public class BgAssetVO : IBgAssetVO
	{
		public int id { get; private set; }
		public AssetPathData assetData { get; private set; }

		public float startPosY { get; set; }
		public float width { get; set; }

		public Vector2 margin { get; set; }
		public Vector2 smooth { get; set; }

		public Vector2 minXAndY { get; set; }
		public Vector2 maxXAndY { get; set; }

		public BgAssetVO (int id1, AssetPathData assetData1)
		{
			id = id1;
			assetData = assetData1;
		}

		public IBgAssetVO clone()
		{
			IBgAssetVO result = new BgAssetVO (id, assetData.clone());
			result.startPosY = startPosY;
			result.width = width;

			result.margin = new Vector2 (margin.x, margin.y);
			result.smooth = new Vector2 (smooth.x, smooth.y);

			result.minXAndY = new Vector2 (minXAndY.x, minXAndY.y);
			result.maxXAndY = new Vector2 (maxXAndY.x, maxXAndY.y);
			return result;
		}
	}
}


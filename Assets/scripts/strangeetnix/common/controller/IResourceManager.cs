using System;
using UnityEngine;

using strangeetnix.game;

namespace strangeetnix
{
	public interface IResourceManager
	{
		int resourceLoadCount { get; }

		void addAssetDataToLoad (AssetPathData assetData);
		void initRequests (PreloaderTypes preloaderType);
		void startLoad ();
		void callbackAfterLoad ();

		GameObject getResourceByAssetData (AssetPathData assetData);
	}
}


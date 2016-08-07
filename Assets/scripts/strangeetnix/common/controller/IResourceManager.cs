using System;
using UnityEngine;

using strangeetnix.game;

namespace strangeetnix
{
	public interface IResourceManager
	{
		void addResorceToLoad (AssetPathData assetData);
		GameObject getResourceById(AssetPathData assetData);
		void startLoad (PreloaderTypes preloaderType);
	}
}


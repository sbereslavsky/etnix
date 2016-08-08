using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strangeetnix.game;
using strangeetnix.ui;

namespace strangeetnix
{
	public class ResourceManager : IResourceManager
	{
		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		[Inject]
		public UpdatePreloaderValueSignal updatePreloaderValueSignal{ get; set; }

		[Inject]
		public DestroyPreloaderSignal destroyPreloaderSignal{ get; set; }

		[Inject]
		public EnterRoomSignal enterRoomSignal{ get; set; }

		[Inject]
		public GameStartSignal gameStartSignal { get; set; }

		[Inject]
		public SwitchCanvasSignal switchCanvasSignal { get; set; }

		private List<AssetPathData> _assetDataList = new List<AssetPathData>();
		private Dictionary<string, ResourceRequest> _resourceDataList = new Dictionary<string, ResourceRequest>();

		private int _count = 0;
		public int resourceLoadCount { get; private set; }
		private PreloaderTypes _preloaderType;

		public void addAssetDataToLoad (AssetPathData assetData)
		{
			if (!_resourceDataList.ContainsKey (assetData.id)) {
				_assetDataList.Add (assetData);
			}
		}

		public void initRequests(PreloaderTypes preloaderType)
		{
			_preloaderType = preloaderType;
			Debug.Log ("ResourceManager.initRequests, type = "+_preloaderType.ToString());
			resourceLoadCount = 0;

			for (int i = 0; i < _assetDataList.Count; i++) {
				initType (_assetDataList[i]);
			}
		}

		public void startLoad()
		{
			Debug.Log ("ResourceManager.startLoad");
			_count = 0;
			foreach (KeyValuePair<string, ResourceRequest> resourceData in _resourceDataList) {
				if (!resourceData.Value.isDone) {					
					routineRunner.StartCoroutine (loadResource (resourceData.Value));
				}
			}
		}

		private void initType(AssetPathData assetData)
		{
			ResourceRequest request = Resources.LoadAsync<GameObject> (assetData.path);
			_resourceDataList.Add (assetData.id, request);
			resourceLoadCount++;
		}

		IEnumerator loadResource(ResourceRequest resourceRequest)
		{
			yield return resourceRequest;

			_count++;
			if (_assetDataList.Count > 0) {
				int percent = (int)(_count * 100 / _assetDataList.Count);
				updatePreloaderValueSignal.Dispatch (percent);

				if (_count == resourceLoadCount) {
					yield return new WaitForSeconds (0.5f);
					//close preloader
					onComplete ();
				}
			} else {
				Debug.LogWarning ("ResourceManager.loadResource with null _assetDataList!");
			}
		}

		private void onComplete()
		{
			clear ();
			destroyPreloaderSignal.Dispatch ();

			callbackAfterLoad ();
		}

		public void callbackAfterLoad()
		{
			switch (_preloaderType) {
			case PreloaderTypes.MAIN:
				gameStartSignal.Dispatch ();
				switchCanvasSignal.Dispatch (UIStates.GAME);
				break;
			case PreloaderTypes.GAME:
				enterRoomSignal.Dispatch (gameModel.waveId);
				break;
			}
		}

		public GameObject getResourceByAssetData(AssetPathData assetData)
		{
			if (_resourceDataList.ContainsKey(assetData.id)) {
				return (GameObject)_resourceDataList [assetData.id].asset;
			}

			return (GameObject) Resources.Load (assetData.path);
		}

		private void clear()
		{
			_count = 0;
			resourceLoadCount = 0;
			_assetDataList.Clear ();
		}
	}
}


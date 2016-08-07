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
		private PreloaderTypes _preloaderType;

		public void addResorceToLoad (AssetPathData assetData)
		{
			if (!_resourceDataList.ContainsKey (assetData.id)) {
				_assetDataList.Add (assetData);
			}
		}

		public GameObject getResourceById(AssetPathData assetData)
		{
			if (_resourceDataList.ContainsKey(assetData.id)) {
				return (GameObject)_resourceDataList [assetData.id].asset;
			}

			return (GameObject) Resources.Load (assetData.path);
		}

		public void startLoad(PreloaderTypes preloaderType)
		{
			_preloaderType = preloaderType;
			for (int i = 0; i < _assetDataList.Count; i++) {
				initType (_assetDataList[i]);
			}

			foreach (KeyValuePair<string, ResourceRequest> resourceData in _resourceDataList) {
				routineRunner.StartCoroutine (loadResource (resourceData.Value));
			}

			//routineRunner.StartCoroutine (onLoading ());
		}

		private void initType(AssetPathData assetData)
		{
			ResourceRequest request = Resources.LoadAsync<GameObject> (assetData.path);
			_resourceDataList.Add (assetData.id, request);
		}

		IEnumerator onLoading()
		{
			int percent = 0;
			while (percent < 100) {
				percent = getSumPercent ();
				updatePreloaderValueSignal.Dispatch (percent);
				yield return new WaitForSeconds(0.1f);
			}

			yield return true;
			onComplete();
		}

		IEnumerator loadResource(ResourceRequest resourceRequest)
		{
			int percent = 0;
			while (!resourceRequest.isDone)
			{
				percent = getSumPercent ();
				updatePreloaderValueSignal.Dispatch (percent);
				yield return null;
			}

			yield return true;
			_count++;
			if (_count == _assetDataList.Count) {
				//close preloader
				onComplete();
			}
		}

		private void onComplete()
		{
			clear ();
			destroyPreloaderSignal.Dispatch ();

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

		private int getSumPercent()
		{
			float sumPercent = 0;
			int count = _resourceDataList.Count;
			foreach (KeyValuePair<string, ResourceRequest> resourceData in _resourceDataList) {
				sumPercent += resourceData.Value.progress;
			}

			float result = sumPercent / count;
			return (int) result * 100;
		}

		private void clear()
		{
			_count = 0;
			_assetDataList.Clear ();
		}
	}
}


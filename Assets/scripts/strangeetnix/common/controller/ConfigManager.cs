using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strangeetnix.game;
using strangeetnix.ui;

namespace strangeetnix
{
	public class ConfigManager : IConfigManager
	{
		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		[Inject]
		public UpdatePreloaderValueSignal updatePreloaderValueSignal{ get; set; }

		[Inject]
		public DestroyPreloaderSignal destroyPreloaderSignal{ get; set; }

		private List<string> _configIdList = new List<string>();
		private Dictionary<string, WWW> _configLoaderList = new Dictionary<string, WWW>();

		private const int PERCENT_COMPLETE = 100;

		private int _count = 0;
		private int _listCount = 0;

		public void startLoad()
		{
			_listCount = GameConfigTypes.list.Count;
			Debug.Log ("ConfigManager.startLoad, count = "+_listCount);

			for (int i = 0; i < _listCount; i++) {
				initLoader (GameConfigTypes.list [i]);
			}
			if (System.IO.File.Exists (gameConfig.persistanceUserDataPath)) {
				gameConfig.readConfigFrom(GameConfigTypes.USER_DATA, gameConfig.persistanceUserDataPath);
			}
			else {
				_listCount++;
				initLoader (GameConfigTypes.USER_DATA);
			}

			Debug.Log ("ConfigManager.initLoaders!");

			foreach (KeyValuePair<string, WWW> configLoader in _configLoaderList) {
				routineRunner.StartCoroutine (loadConfig (configLoader.Key, configLoader.Value));
			}

			//routineRunner.StartCoroutine (updateStatus());
		}

		private void initLoader(string id) 
		{
			string resultPath = gameConfig.getConfigPath (id);
			WWW configLoader = new WWW(resultPath);
			_configLoaderList.Add (id, configLoader);
		}

		IEnumerator updateStatus()
		{
			int percent = 0;
			while (percent < PERCENT_COMPLETE) {
				percent = getSumPercent ();
				Debug.Log ("ConfigManager.updateStatus, percent = "+percent.ToString());
				updatePreloaderValueSignal.Dispatch (percent); 
				yield return 0;
			}

			yield return true;
			Debug.Log ("ConfigManager.updateStatus, complete!!!");
			foreach (KeyValuePair<string, WWW> configLoader in _configLoaderList) {
				if (configLoader.Value.error == null) {
					Debug.Log ("ConfigManager.updateStatus, parseToData, key = "+configLoader.Key);
					gameConfig.parseStreamToJSON (configLoader.Value, configLoader.Key);
				} else {
					Debug.Log ("ConfigManager.updateStatus, error = " + configLoader.Value.error + ". load key = "+configLoader.Key);
					gameConfig.readConfigFrom (configLoader.Key, null, true);
				}
			}

			clear ();
			destroyPreloaderSignal.Dispatch ();
			gameConfig.loadedDataSignal.Dispatch ();
		}

		IEnumerator loadConfig(string id, WWW configLoader)
		{
			int percent = 0;
			while (!configLoader.isDone)
			{
				percent = getSumPercent ();
				updatePreloaderValueSignal.Dispatch (percent); 
				yield return null;
			}

			yield return true;
			percent = getSumPercent ();
			updatePreloaderValueSignal.Dispatch (percent); 

			if (configLoader.error == null) {
				gameConfig.parseStreamToJSON (configLoader, id);
			} else {
				gameConfig.readConfigFrom (id);
			}

			_count++;
			if (_count == _listCount) {
				//close preloader
				clear ();
				destroyPreloaderSignal.Dispatch ();
				gameConfig.loadedDataSignal.Dispatch ();
			}
		}

		private int getSumPercent()
		{
			float sumPercent = 0;
			int count = _configLoaderList.Count;
			foreach (KeyValuePair<string, WWW> configLoader in _configLoaderList) {
				if (configLoader.Value.error != null) {
					count--;
				} else {
					sumPercent += configLoader.Value.progress;
				}
			}

			float result = sumPercent / count;
			return (int) result * 100;
		}

		private void clear()
		{
			_configLoaderList.Clear ();
			_configIdList.Clear ();
		}
	}
}


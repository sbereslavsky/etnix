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
		}

		private void initLoader(string id) 
		{
			string resultPath = gameConfig.getConfigPath (id, true);
			WWW configLoader = new WWW(resultPath);
			_configLoaderList.Add (id, configLoader);
		}

		IEnumerator loadConfig(string id, WWW configLoader)
		{
			yield return configLoader;

			_count++;
			int percent = (int) (_count*100/_listCount);
			updatePreloaderValueSignal.Dispatch (percent); 

			if (configLoader.error == null) {
				gameConfig.parseStreamToJSON (configLoader, id);
			} else {
				gameConfig.readConfigFrom (id);
			}

			if (_count == _listCount) {
				yield return new WaitForSeconds (0.5f);
				//close preloader
				clear ();
				destroyPreloaderSignal.Dispatch ();
				gameConfig.loadedDataSignal.Dispatch ();
			}
		}

		private void clear()
		{
			_configLoaderList.Clear ();
			_configIdList.Clear ();
		}
	}
}


// клас для работы с json данными

using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class GameConfig : IGameConfig
	{  
		public Signal loadedDataSignal { get; private set; }
		public string persistanceUserDataPath { get; private set; }

		private string _dataPath;

		private string _userDataPath;

		public ILevelConfig levelConfig { get; private set; }
		public IAssetConfig assetConfig { get; private set; }

		public ICharInfoConfig charInfoConfig { get; private set; }
		public ICharABConfig charAbConfig { get; private set; }
		public ICharAllConfig charAllConfig { get; private set; }
		public IEnemyConfig enemyConfig { get; private set; }
		public IItemConfig itemConfig { get; private set; }
		public IWaveConfig waveConfig { get; private set; }
		public IWeaponConfig weaponConfig { get; private set; }
		public IEquipedConfig equipedConfig { get; private set; }
		public ILocalizationConfig localizationConfig { get; private set; }

		public IUserCharConfig userCharConfig { get; private set; }

		private const int MAX_CONFIG = 10;
		private int _configNum = 0;

		public GameConfig()
		{	
			loadedDataSignal = new Signal ();

			initPathes ();

			levelConfig = new LevelConfig ();
			assetConfig = new AssetConfig ();
		}

		private void initPathes()
		{
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			_dataPath = Application.streamingAssetsPath + "/data/";
			_userDataPath = Application.streamingAssetsPath + "/userData/";
			#endif

			#if UNITY_ANDROID && !UNITY_EDITOR
			persistanceUserDataPath = Application.persistentDataPath + "/" + GameConfigTypes.USER_DATA + ".json";
			_dataPath =  "jar:file://" + Application.dataPath + "!/assets/data/";
			_userDataPath = "jar:file://" + Application.dataPath + "!/assets/userData/";
			#endif

			#if UNITY_IOS
			_dataPath = Application.dataPath + "/Raw/data/";
			_userDataPath = Application.dataPath + "/Raw/userData/";
			#endif
		}

		public void loadLocalConfigs()
		{
			for (int i = 0; i < GameConfigTypes.list.Count; i++) {
				readConfigFrom (GameConfigTypes.list[i]);
			}

			readConfigFrom(GameConfigTypes.USER_DATA);
		}

		public string getConfigPath(string configName, bool throughWww=false)
		{
			string configPath = (configName == GameConfigTypes.USER_DATA) ? _userDataPath : _dataPath;
			string resultPath = configPath + configName + ".json";
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (throughWww) {
				resultPath = "file:///" + resultPath;
			}
			#endif

			return resultPath;
		}

		//создание обьекта с которым можем работать из файла json
		public void readConfigFrom(string id, string path=null, bool throughWww=false) 
		{
			string configPath = (path != null) ? path : getConfigPath (id, throughWww);
			if (System.IO.File.Exists (configPath)) {
				string linkstream = System.IO.File.ReadAllText (configPath);
				JSONObject jsonConfig = new JSONObject (linkstream);
				initConfig (id, jsonConfig, true);
			} else {
				Debug.LogError ("Can't read " + id + " config!");
			}
		}

		public void parseStreamToJSON(WWW stream, string id)
		{
			if (!string.IsNullOrEmpty (stream.error)) {
				Debug.LogError ("Can't read " + id + " config!");
			} else {
				JSONObject jsonConfig = new JSONObject(stream.text);
				initConfig (id, jsonConfig, false);
			}
		}			

		private void initConfig(string id, JSONObject jsonConfig, bool readFromFile)
		{
			switch (id) {
			case GameConfigTypes.CHAR_INFO:
				charInfoConfig = new CharInfoConfig(jsonConfig);
				break;
			case GameConfigTypes.CHAR_AB:
				charAbConfig = new CharABConfig(jsonConfig);
				break;
			case GameConfigTypes.CHAR_ALL:
				charAllConfig = new CharAllConfig(jsonConfig);
				break;
			case GameConfigTypes.ENEMY:
				enemyConfig = new EnemyConfig(jsonConfig);
				break;
			case GameConfigTypes.ITEMS:
				itemConfig = new ItemConfig(jsonConfig);
				break;
			case GameConfigTypes.WAVES:
				waveConfig = new WaveConfig(jsonConfig);
				break;
			case GameConfigTypes.WEAPONS:
				weaponConfig = new WeaponConfig(jsonConfig);
				break;
			case GameConfigTypes.EQUIPED:
				equipedConfig = new EquipedConfig(jsonConfig);
				break;
			case GameConfigTypes.LOCALIZATION:
				localizationConfig = new LocalizationConfig(jsonConfig);
				break;
			case GameConfigTypes.USER_DATA:
				userCharConfig = new UserCharConfig (jsonConfig);
				break;
			}

			if (readFromFile) {
				_configNum++;
				if (_configNum == MAX_CONFIG) {
					loadedDataSignal.Dispatch ();
				}
			}
		}

		public void save()
		{
			JSONObject userData = userCharConfig.getJSONObject ();
			string text = userData.Print (true);
			#if !UNITY_ANDROID || UNITY_EDITOR
			System.IO.File.WriteAllText (getConfigPath(GameConfigTypes.USER_DATA), text);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			try {
				System.IO.File.WriteAllText (persistanceUserDataPath, text);
			}
			catch(Exception error) {
				Debug.LogError ("Can't save file!");
			}
			#endif
		}
	}
}
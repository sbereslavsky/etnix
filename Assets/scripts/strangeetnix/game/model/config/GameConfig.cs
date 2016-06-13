// клас для работы с json данными

using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public class GameConfig : IGameConfig
	{  
		public Signal loadedDataSignal { get; private set; }

		private string _dataPath;
		private string _persistanceUserDataPath;
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
		}

		private void initPathes()
		{
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			_dataPath = Application.dataPath + "/StreamingAssets/data/";
			_userDataPath = Application.dataPath + "/StreamingAssets/userData/";
			#endif

			#if UNITY_ANDROID && !UNITY_EDITOR
			_persistanceUserDataPath = Application.persistentDataPath + "/" + GameConfigTypes.USER_DATA + ".json";
			_dataPath =  "jar:file://" + Application.dataPath + "!/assets/data/";
			_userDataPath = "jar:file://" + Application.dataPath + "!/assets/userData/";
			#endif

			#if UNITY_IOS
			_dataPath = Application.dataPath + "/Raw/data/";
			_userDataPath = Application.dataPath + "/Raw/userData/";
			#endif
		}

		public void Load(IRoutineRunner routineRunner)
		{
			initPathes ();

			levelConfig = new LevelConfig ();
			assetConfig = new AssetConfig ();

			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
				readConfigFrom(GameConfigTypes.CHAR_INFO);
				readConfigFrom(GameConfigTypes.CHAR_AB);
				readConfigFrom(GameConfigTypes.CHAR_ALL);
				readConfigFrom(GameConfigTypes.ENEMY);
				readConfigFrom(GameConfigTypes.ITEMS);
				readConfigFrom(GameConfigTypes.WAVES);
				readConfigFrom(GameConfigTypes.WEAPONS);
				readConfigFrom(GameConfigTypes.EQUIPED);
				readConfigFrom(GameConfigTypes.LOCALIZATION);
				readConfigFrom(GameConfigTypes.USER_DATA);
			#elif UNITY_ANDROID
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.CHAR_INFO));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.CHAR_AB));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.CHAR_ALL));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.ENEMY));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.ITEMS));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.WAVES));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.WEAPONS));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.EQUIPED));
				routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.LOCALIZATION));
				if (System.IO.File.Exists (_persistanceUserDataPath)) {
					readConfigFrom(GameConfigTypes.USER_DATA, _persistanceUserDataPath);
				}
				else {
					routineRunner.StartCoroutine (loadConfigCoroutine (GameConfigTypes.USER_DATA));
				}
				
			#endif
		}

		private string getConfigPath(string configName)
		{
			string configPath = (configName == GameConfigTypes.USER_DATA) ? _userDataPath : _dataPath;
			string resultPath = configPath + configName + ".json";
			return resultPath;
		}

		//создание обьекта с которым можем работать из файла json
		private void readConfigFrom(string id, string path=null) 
		{
			string configPath = (path != null) ? path : getConfigPath (id);
			if (System.IO.File.Exists (configPath)) {
				string linkstream = System.IO.File.ReadAllText (configPath);
				JSONObject jsonConfig = new JSONObject (linkstream);
				initConfig (id, jsonConfig);
			} else {
				Debug.LogError ("Can't read " + id + " config!");
			}
		}

		IEnumerator loadConfigCoroutine(string id)
		{
			string resultPath = getConfigPath (id);
			WWW linkstream = new WWW(resultPath);
			yield return linkstream;
			if (!string.IsNullOrEmpty (linkstream.error)) {
				Debug.LogError ("Can't read " + id + " config!");
			} else {
				JSONObject jsonConfig = new JSONObject(linkstream.text);
				initConfig (id, jsonConfig);
			}
		}

		private void initConfig(string id, JSONObject jsonConfig)
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

			_configNum++;
			if (_configNum == MAX_CONFIG) {
				loadedDataSignal.Dispatch();
			}
		}

		public void Save()
		{
			JSONObject userData = userCharConfig.getJSONObject ();
			string text = userData.Print (true);
			#if !UNITY_ANDROID || UNITY_EDITOR
			System.IO.File.WriteAllText (getConfigPath(GameConfigTypes.USER_DATA), text);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			try {
				System.IO.File.WriteAllText (_persistanceUserDataPath, text);
			}
			catch(Exception error) {
				Debug.LogError ("Can't save file!");
			}
			#endif
		}
	}
}
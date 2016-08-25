using System;
using UnityEngine;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class AssetConfig : IAssetConfig
	{
		private const string FOLDER_PLAYER 			= "player/";
		private const string FOLDER_ENEMY 			= "enemy/";
		private const string FOLDER_BACKGROUND 		= "bg/";
		private const string FOLDER_UI 				= "ui/";
		private const string FOLDER_FX 				= "fx/";

		private AssetPathData PLAYER_SIRKO 			= new AssetPathData ("Sirko", FOLDER_PLAYER);
		private AssetPathData PLAYER_NELYA 			= new AssetPathData ("Nelya", FOLDER_PLAYER);

		private AssetPathData ENEMY_DICKENS 		= new AssetPathData ("Dickens", FOLDER_ENEMY);
		private AssetPathData ENEMY_MAVKA 			= new AssetPathData ("Mavka", FOLDER_ENEMY);
		private AssetPathData ENEMY_VODYANOJ 		= new AssetPathData ("Vodyanoj", FOLDER_ENEMY);
		private AssetPathData ENEMY_DIDKO 			= new AssetPathData ("Didko", FOLDER_ENEMY);

		private AssetPathData BACKGROUND_VILLAGE 	= new AssetPathData ("village", FOLDER_BACKGROUND);
		private AssetPathData BACKGROUND_CHURCH 	= new AssetPathData ("church", FOLDER_BACKGROUND);

		public static AssetPathData DIALOG_CHAR_INFO 	= new AssetPathData ("DialogCharInfo", FOLDER_UI);
		public static AssetPathData DIALOG_CHAR_EDIT 	= new AssetPathData ("DialogCharEdit", FOLDER_UI);
		public static AssetPathData DIALOG_CHAR_LIST 	= new AssetPathData ("DialogCharList", FOLDER_UI);

		public static AssetPathData DIALOG_PAUSE_GAME 	= new AssetPathData ("DialogPauseGame", FOLDER_UI);
		public static AssetPathData DIALOG_WIN_GAME 	= new AssetPathData ("DialogWinGame", FOLDER_UI);
		public static AssetPathData DIALOG_LOSE_GAME 	= new AssetPathData ("DialogLoseGame", FOLDER_UI);
		public static AssetPathData DIALOG_CHOOSE_WAVE 	= new AssetPathData ("DialogChooseWave", FOLDER_UI);
		public static AssetPathData DIALOG_WEAPON_EDIT 	= new AssetPathData ("DialogWeaponEdit", FOLDER_UI);

		public static AssetPathData PRELOADER		= new AssetPathData ("Preloader", FOLDER_UI);
		public static AssetPathData EXPLOSION		= new AssetPathData ("explosion", FOLDER_FX);

		public static AssetPathData CANVAS_MAIN		= new AssetPathData ("MainCanvas", FOLDER_UI);
		public static AssetPathData CANVAS_GAME		= new AssetPathData ("GameCanvas", FOLDER_UI);

		private Dictionary<int, ICharAssetVO> _playerAssetList;
		private Dictionary<int, IBgAssetVO> _bgAssetList;

		public Dictionary<int, ICharAssetVO> enemyAssetList { get; private set; }

		public List<AssetPathData> villageAssetDataList { get; private set; } 
		public List<AssetPathData> churchAssetDataList { get; private set; }

		public AssetConfig ()
		{
			_playerAssetList = new Dictionary<int, ICharAssetVO>();
			_playerAssetList.Add (1, new CharAssetVO (1, PLAYER_SIRKO, 0.3f, 2f,	true));
			_playerAssetList.Add (2, new CharAssetVO (2, PLAYER_NELYA, 0.4f, 2f,	true));

			enemyAssetList = new Dictionary<int, ICharAssetVO> ();
			enemyAssetList.Add (1, new CharAssetVO (1, ENEMY_DICKENS, 1.5f, 1.7f,	false));
			enemyAssetList.Add (2, new CharAssetVO (2, ENEMY_MAVKA, 0.7f, 1.7f,	true));
			enemyAssetList.Add (3, new CharAssetVO (3, ENEMY_VODYANOJ, 1f, 	1.3f,	false));
			enemyAssetList.Add (4, new CharAssetVO (4, ENEMY_DIDKO, 0.7f, 2.3f, 	true));

			_bgAssetList = new Dictionary<int, IBgAssetVO> ();
			IBgAssetVO bgAssetVO = new BgAssetVO (1, BACKGROUND_VILLAGE);
			bgAssetVO.startPosY = -1.24f;
			bgAssetVO.width = 97f;
			bgAssetVO.margin = Vector2.zero;
			bgAssetVO.smooth = new Vector2 (4, 0);
			bgAssetVO.minXAndY = new Vector2 (-7.7f, 0f); //-25
			bgAssetVO.maxXAndY = new Vector2 (9.5f, 0f); //25
			_bgAssetList.Add (1, bgAssetVO);

			bgAssetVO = new BgAssetVO (1001, BACKGROUND_CHURCH);
			bgAssetVO.startPosY = -1.68f;
			bgAssetVO.width = 28.4f;
			bgAssetVO.margin = Vector2.zero;
			bgAssetVO.smooth = new Vector2 (4, 0);
			bgAssetVO.minXAndY = new Vector2 (-2.25f, 0f); //-25
			bgAssetVO.maxXAndY = new Vector2 (2.25f, 0f); //25

			_bgAssetList.Add (1001, bgAssetVO);
		}

		public void initMainAssets (int playerId)
		{
			if (villageAssetDataList == null) {
				villageAssetDataList = new List<AssetPathData> ();
			}
			else if (villageAssetDataList.Count > 0) {
				villageAssetDataList.Clear ();
			}

			villageAssetDataList.Add (BACKGROUND_VILLAGE);

			//add player path
			if (_playerAssetList.ContainsKey (playerId) && !villageAssetDataList.Contains(_playerAssetList [playerId].assetData)) {
				villageAssetDataList.Add (_playerAssetList [playerId].assetData);
			}

			//add dialogs
			villageAssetDataList.Add (DIALOG_CHOOSE_WAVE);
			villageAssetDataList.Add (DIALOG_PAUSE_GAME);
		}

		public void initGameAssets (List<int> enemyIds)
		{
			if (churchAssetDataList == null) {
				churchAssetDataList = new List<AssetPathData> ();
			}
			else if (churchAssetDataList.Count > 0) {
				churchAssetDataList.Clear ();
			}

			churchAssetDataList.Add (BACKGROUND_CHURCH);
			churchAssetDataList.Add (EXPLOSION);

			for (int i = 0; i < enemyIds.Count; i++) {
				int id = enemyIds [i];
				AssetPathData assetData = enemyAssetList [id].assetData;
				if (!churchAssetDataList.Contains (assetData)) {
					churchAssetDataList.Add (assetData);
				}
			}

			churchAssetDataList.Add (DIALOG_WIN_GAME);
			churchAssetDataList.Add (DIALOG_LOSE_GAME);
		}

		public ICharAssetVO getPlayerAssetById(int id)
		{
			return getCharAssetById (_playerAssetList, id);
		}

		public ICharAssetVO getEnemyAssetById(int id)
		{
			return getCharAssetById (enemyAssetList, id);
		}

		public IBgAssetVO getBgAssetById(int id)
		{
			return getBackgroundAssetById (_bgAssetList, id);
		}

		private ICharAssetVO getCharAssetById(Dictionary<int, ICharAssetVO> list, int id)
		{
			if (list.ContainsKey (id)) {
				return list [id];
			}

			return null;
		}

		private IBgAssetVO getBackgroundAssetById(Dictionary<int, IBgAssetVO> list, int id)
		{
			if (list.ContainsKey (id)) {
				return list [id];
			}

			return null;
		}
	}

	public class AssetPathData
	{
		public string id { get; private set; }
		public string path { get; private set; }
		public string folder { get; private set; }

		public AssetPathData (string id1, string folder1)
		{
			id = id1;
			folder = folder1;
			path = folder1 + id1;
		}

		public AssetPathData clone()
		{
			return new AssetPathData (id, folder);
		}
	}
}


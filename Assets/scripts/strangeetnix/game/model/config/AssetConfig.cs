using System;
using UnityEngine;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class AssetConfig : IAssetConfig
	{
		private const string PLAYER_FOLDER 		= "player/";
		private const string ENEMY_FOLDER 		= "enemy/";
		private const string BACKGROUND_FOLDER 	= "bg/";

		private const string PLAYER_SIRKO 		= "Sirko";
		private const string PLAYER_NELYA 		= "Nelya";

		private const string ENEMY_DICKENS 		= "Dickens";
		private const string ENEMY_MAVKA 		= "Mavka";
		private const string ENEMY_VODYANOJ 	= "Vodyanoj";
		private const string ENEMY_DIDKO 		= "Didko";

		private const string BACKGROUND_LEVEL1 	= "village";
		private const string BACKGROUND_CHURCH 	= "church";

		private List<ICharAssetVO> _playerAssetList;
		private List<IBgAssetVO> _bgAssetList;

		public List<ICharAssetVO> enemyAssetList { get; private set; }

		public AssetConfig ()
		{
			_playerAssetList = new List<ICharAssetVO>();
			_playerAssetList.Add (new CharAssetVO (1, PLAYER_SIRKO, PLAYER_FOLDER, 0.3f, 2f,	true));
			_playerAssetList.Add (new CharAssetVO (2, PLAYER_NELYA, PLAYER_FOLDER, 0.4f, 2f,	true));

			enemyAssetList = new List<ICharAssetVO> ();
			enemyAssetList.Add (new CharAssetVO (1, ENEMY_DICKENS, 	ENEMY_FOLDER, 1.5f, 1.7f,	false));
			enemyAssetList.Add (new CharAssetVO (2, ENEMY_MAVKA, 	ENEMY_FOLDER, 0.7f, 1.7f,	true));
			enemyAssetList.Add (new CharAssetVO (3, ENEMY_VODYANOJ, ENEMY_FOLDER, 1f, 	1.3f,	false));
			enemyAssetList.Add (new CharAssetVO (4, ENEMY_DIDKO, 	ENEMY_FOLDER, 0.7f, 2.3f, 	true));

			_bgAssetList = new List<IBgAssetVO> ();
			IBgAssetVO bgAssetVO = new BgAssetVO (1, BACKGROUND_LEVEL1, BACKGROUND_FOLDER + BACKGROUND_LEVEL1);
			bgAssetVO.startPosY = -1.24f;
			bgAssetVO.width = 97f;
			bgAssetVO.margin = Vector2.zero;
			bgAssetVO.smooth = new Vector2 (4, 0);
			bgAssetVO.minXAndY = new Vector2 (-7.7f, 0f); //-25
			bgAssetVO.maxXAndY = new Vector2 (9.5f, 0f); //25
			_bgAssetList.Add (bgAssetVO);

			bgAssetVO = new BgAssetVO (1001, BACKGROUND_CHURCH, BACKGROUND_FOLDER + BACKGROUND_CHURCH);
			bgAssetVO.startPosY = -1.68f;
			bgAssetVO.width = 28.4f;
			bgAssetVO.margin = Vector2.zero;
			bgAssetVO.smooth = new Vector2 (4, 0);
			bgAssetVO.minXAndY = new Vector2 (-5.5f, 0f); //-25
			bgAssetVO.maxXAndY = new Vector2 (5.5f, 0f); //25

			_bgAssetList.Add (bgAssetVO);
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
			return getBgAssetById (_bgAssetList, id);
		}

		private ICharAssetVO getCharAssetById(List<ICharAssetVO> list, int id)
		{
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].id == id) {
					return list [i];
				}
			}

			return null;
		}

		private IBgAssetVO getBgAssetById(List<IBgAssetVO> list, int id)
		{
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].id == id) {
					return list [i];
				}
			}

			return null;
		}
	}
}


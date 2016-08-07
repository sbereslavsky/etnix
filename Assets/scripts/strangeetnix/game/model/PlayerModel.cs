using System;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class PlayerModel : IPlayerModel 
	{
		public int id { get; private set; }
		public string name { get; private set; }

		public int hp { get; set; }
		public int startHp { get; private set; }
		public int exp { get; set; }
		public int expStart { get; private set; }
		public int expEnd { get; private set; }
		public float moveSpeed { get; private set; }
		public float moveForce { get; private set; }
		public int level { get; set; }

		public int damage { get { return Mathf.Max (0, _damage + char_str);}}
		public int cooldown { get{ return Mathf.Max (0, _cooldown - char_dex);}}

		public bool levelUp { get; private set; }

		public ICharAssetVO assetVO { get; private set; }
		public IEquipedVO equipedVO { get; set; }
		public IWeaponVO weaponVO { get; set; }
		public IItemVO item2VO { get; set; }
		public IItemVO item3VO { get; set; }

		private int _damage = 0;
		private int _cooldown = 0;

		private IUserCharVO _userCharVO;
		private ICharAllVO _levelDataVO;

		public PlayerModel(int setId, IGameConfig gameConfig)
		{
			id = setId;
			assetVO = gameConfig.assetConfig.getPlayerAssetById (id);
			name = assetVO.assetData.id;
			levelUp = false;

			ICharInfoVO charInfoVO = gameConfig.charInfoConfig.getCharInfoVOById (id);
			moveSpeed = charInfoVO.speed;
			moveForce = charInfoVO.moveForce;
			
			parseData (gameConfig);
		}

		private int char_str { get { return (id == 1) ? _levelDataVO.ch1_str : _levelDataVO.ch2_str; } }
		private int char_dex { get { return (id == 1) ? _levelDataVO.ch1_dex : _levelDataVO.ch2_dex; } }
		private int char_hp { get { return (id == 1) ? _levelDataVO.ch1_hp : _levelDataVO.ch2_hp; } }

		private void parseData(IGameConfig gameConfig)
		{
			_userCharVO = gameConfig.userCharConfig.getUserCharVOById (id);

			exp = _userCharVO.exp;

			updateNextExp (gameConfig);

			equipedVO = gameConfig.equipedConfig.getEquipedVOById (_userCharVO.equipedId);
			weaponVO = gameConfig.weaponConfig.getWeaponVOById (_userCharVO.weaponId);
			if (weaponVO != null) {
				_damage = weaponVO.damage;
				_cooldown = weaponVO.cooldown;
			}

			item2VO = gameConfig.itemConfig.getItemVOById (_userCharVO.itemId2);
			item3VO = gameConfig.itemConfig.getItemVOById (_userCharVO.itemId3);
			//waveVO = gameConfig.waveConfig.getWaveVOById (_userCharVO.waveId);


		}

		public void addHp(int value)
		{
			hp = Mathf.Min (hp + value, startHp);
		}

		public void decHp(int value)
		{
			hp = Mathf.Max (hp - value, 0);
		}

		public void addExp(int value)
		{
			exp += value;

			if (exp >= expEnd) {
				levelUp = true;
			}
		}

		public void updateConfigExp()
		{
			if (_userCharVO != null) {
				_userCharVO.exp = exp;
			}
		}

		public void updateNextExp(IGameConfig gameConfig)
		{
			_levelDataVO = gameConfig.charAllConfig.getCharAllVOByExp (exp);
			level = _levelDataVO.level_id;
			expStart = _levelDataVO.exp_next;

			Debug.Log ("updateNextExp: level = " + level + ", expStart = expStart");

			ICharAllVO nextLevelDataVO = gameConfig.charAllConfig.getCharAllVOByLevel (level+1);
			if (nextLevelDataVO != null) {
				expEnd = nextLevelDataVO.exp_next;
			} else {
				expEnd += 500;
				Debug.Log ("updateNextExp: nextLevelDataVO = null!");
			}

			hp = char_hp;
			startHp = hp;

			levelUp = false;
		}

		public void saveData()
		{
			_userCharVO.exp = exp;
		}

		public void resetHp ()
		{
			hp = startHp;
		}
	}
}
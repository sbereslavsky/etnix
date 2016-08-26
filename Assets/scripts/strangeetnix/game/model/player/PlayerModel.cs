using System;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class PlayerModel : WeaponModel, IPlayerModel
	{
		public string name { get; private set; }

		public float moveSpeed { get; private set; }
		public float moveForce { get; private set; }

		public int coins { get { return _coins; }}

		private int _coins = 0;

		public ICharAssetVO assetVO { get; private set; }

		public PlayerModel (int setId, IGameConfig gameConfig) : base (setId, gameConfig)
		{
			assetVO = gameConfig.assetConfig.getPlayerAssetById (setId);
			name = assetVO.assetData.id;

			ICharInfoVO charInfoVO = gameConfig.charInfoConfig.getCharInfoVOById (setId);
			moveSpeed = charInfoVO.speed;
			moveForce = charInfoVO.moveForce;

			_coins = userCharVO.coins;

			setLevelDataVO ();
			setEndExp ();
		}

		public void updateUserConfig()
		{
			if (userCharVO != null) {
				userCharVO.exp = exp;
				userCharVO.coins = _coins;
			}
		}

		public void addCoins(int value)
		{
			_coins += value;
		}
	}
}
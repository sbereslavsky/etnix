using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IPlayerModel : IWeaponModel, IUserModel
	{
		string name { get; }

		int coins { get; }
		float moveSpeed { get; }
		float moveForce { get; }

		ICharAssetVO assetVO { get; }

		void addCoins (int value);

		void updateUserConfig ();
	}
}
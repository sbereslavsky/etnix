using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public interface IGameConfig
	{
		Signal loadedDataSignal { get; }

		void Load (IRoutineRunner routineRunner);
		void Save ();

		ILevelConfig levelConfig { get; }
		IAssetConfig assetConfig { get; }

		ICharInfoConfig charInfoConfig { get; }
		ICharABConfig charAbConfig { get; }
		ICharAllConfig charAllConfig { get; }
		IEnemyConfig enemyConfig { get; }
		IItemConfig itemConfig { get; }
		IWaveConfig waveConfig { get; }
		IWeaponConfig weaponConfig { get; }
		IEquipedConfig equipedConfig { get; }
		ILocalizationConfig localizationConfig { get; }

		IUserCharConfig userCharConfig { get; }
	}
}
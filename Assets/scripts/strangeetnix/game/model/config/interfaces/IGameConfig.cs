using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public interface IGameConfig
	{
		Signal loadedDataSignal { get; }

		string persistanceUserDataPath { get; }

		void loadLocalConfigs ();
		void readConfigFrom (string id, string path = null, bool throughWww=false);
		void parseStreamToJSON (WWW stream, string id);
		string getConfigPath (string configName, bool throughWww=false);

		void save ();

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
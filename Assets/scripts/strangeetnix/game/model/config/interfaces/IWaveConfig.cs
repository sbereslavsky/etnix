using System;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IWaveConfig
	{
		int count { get; }
		List<string> info_names { get; }
		string getEncounterListById (int id);
		int getIdByEncounterList (string value);
		float getEncounterSpeedById (int id);
		IWaveVO getWaveVOById(int id);
	}
}
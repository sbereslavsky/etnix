using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IWaveVO
	{
		int id { get; }
		float encounter_speed { get; }
		int defence_hp { get; }
		int defence_speed { get; }
		string enemy_encounter_id_list_str { get; }
		List<int> enemy_encounter_id_list { get; }
		List<int> enemy_unique_id_list { get; }
	}
}
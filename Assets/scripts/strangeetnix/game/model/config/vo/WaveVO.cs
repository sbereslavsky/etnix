using System;
using System.Linq;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class WaveVO : ParseJSONObject, IWaveVO
	{
		private string WAVE_ID 					= "waveid";
		private string ENCOUNTER_SPEED 			= "encounterspeed";
		private string DEFENCE_HP 				= "defencehp";
		private string DEFENCE_SPEED 			= "defencespeed";
		private string ENEMY_ENCOUNTER_ID_LIST 	= "enemyencounteridlist";

		public int id { get; private set;}
		public float encounter_speed { get; private set;}
		public int defence_hp { get; private set;}
		public int defence_speed { get; private set;}
		public string enemy_encounter_id_list_str { get; private set; }
		public List<int> enemy_encounter_id_list { get; private set; }
		public List<int> enemy_unique_id_list { get; private set; }

		public WaveVO (JSONObject value)
		{
			id = getInt32(value, WAVE_ID);
			encounter_speed = getFloat(value, ENCOUNTER_SPEED);
			defence_hp = getInt32(value, DEFENCE_HP);
			defence_speed = getInt32(value, DEFENCE_SPEED);
			enemy_encounter_id_list_str = getString(value, ENEMY_ENCOUNTER_ID_LIST);
			enemy_encounter_id_list = convertStringToList (enemy_encounter_id_list_str);

			HashSet<int> hashIdList = new HashSet<int> (enemy_encounter_id_list);
			enemy_unique_id_list = hashIdList.ToList();
		}
	}
}

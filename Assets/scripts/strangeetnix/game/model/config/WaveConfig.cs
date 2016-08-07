using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class WaveConfig : IWaveConfig
	{
		private List<IWaveVO> _list;

		public List<string> info_names { get; private set;}

		public int count { get; private set; }

		public WaveConfig (JSONObject value)
		{
			count = value.Count;
			_list = new List<IWaveVO> (value.Count);
			info_names = new List<string> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new WaveVO (value [i]));
				info_names.Add (_list[i].enemy_encounter_id_list_str);
			}
		}

		public string getEncounterListById(int id)
		{
			IWaveVO waveVO = getWaveVOById (id);
			string result = (waveVO != null) ? waveVO.enemy_encounter_id_list_str : "";
			return result;
		}

		public float getEncounterSpeedById(int id)
		{
			IWaveVO waveVO = getWaveVOById (id);
			float result = (waveVO != null) ? waveVO.encounter_speed : 0;
			return result;
		}

		public IWaveVO getWaveVOById(int id)
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].id == id) {
					return _list [i];
				}
			}

			return null;
		}

		public int getIdByEncounterList(string value)
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].enemy_encounter_id_list_str == value) {
					return _list [i].id;
				}
			}

			return -1;
		}
	}
}
using System;

namespace strangeetnix.game
{
	public class LocalizationVO: ParseJSONObject, ILocalizationVO
	{
		private string KEY_ID = "key";
		private string EN = "en";
		private string RU = "ru";

		public string key { get; private set;}
		public string en { get; private set;}
		public string ru { get; private set;}

		public LocalizationVO (JSONObject value)
		{
			key = getString(value, KEY_ID);
			en = getString(value, EN);
			ru = getString(value, RU);
		}
	}
}
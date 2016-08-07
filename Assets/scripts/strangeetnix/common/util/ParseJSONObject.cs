using System;
using System.Collections.Generic;

public class ParseJSONObject
{
	public ParseJSONObject ()
	{
	}

	protected string getString(JSONObject jsonData, string id)
	{
		string result = "";
		if (!jsonData.HasField (id)) {
			return result;
		}

		JSONObject fieldData = jsonData.GetField (id);
		if (fieldData.IsString) {
			result = fieldData.str;
		} else if (fieldData.IsNumber) {
			result = Convert.ToString (fieldData.i);
		}

		return result;
	}

	protected int getInt32(JSONObject jsonData, string id)
	{
		int result = 0;
		if (!jsonData.HasField (id)) {
			return result;
		}

		JSONObject fieldData = jsonData.GetField (id);
		if (fieldData.IsString) {
			result = Convert.ToInt32(fieldData.str);
		} else if (fieldData.IsNumber) {
			result = Convert.ToInt32 (fieldData.i);
		}

		return result;
	}

	protected float getFloat(JSONObject jsonData, string id)
	{
		float result = 0;
		if (!jsonData.HasField (id)) {
			return result;
		}

		JSONObject fieldData = jsonData.GetField (id);
		if (fieldData.IsString) {
			result = (float) Convert.ToDouble(fieldData.str);
		} else if (fieldData.IsNumber) {
			result = (float) Convert.ToDouble (fieldData.f);
		}

		return result;
	}

	protected void setIntToField(JSONObject jsonData, string id, int value)
	{
		if (!jsonData.HasField (id)) {
			return;
		}

		JSONObject fieldData = jsonData.GetField (id);
		if (fieldData.IsString) {
			fieldData.str = value.ToString ();
		} else if (fieldData.IsNumber) {
			fieldData.i = value;
		}
	}

	protected void setStringToField(JSONObject jsonData, string id, string value)
	{
		if (!jsonData.HasField (id)) {
			return;
		}

		JSONObject fieldData = jsonData.GetField (id);
		if (fieldData.IsString) {
			fieldData.str = value;
		} else if (fieldData.IsNumber) {
			fieldData.i = Convert.ToInt32 (value);
		}
	}

	protected List<int> convertStringToList(string value)
	{
		List<int> result = new List<int> ();
		if (value != null && value.Length > 0) {
			string[] ids = value.Split (',');
			for (byte i = 0; i < ids.Length; i++) {
				result.Add (Convert.ToInt32(ids.GetValue(i)));
			}
		}
		return result;
	}
}


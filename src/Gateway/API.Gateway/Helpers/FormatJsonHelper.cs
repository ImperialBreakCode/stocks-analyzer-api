﻿using Newtonsoft.Json;
using Serilog;

public static class FormatJsonHelper
{
	public static string FormatJson(string json)
	{
		try
		{
			dynamic parsedJson = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}
		catch (JsonException ex)
		{
			Log.Error($"Error formating JSON: {ex.Message}");
			return json;
		}
	}
}